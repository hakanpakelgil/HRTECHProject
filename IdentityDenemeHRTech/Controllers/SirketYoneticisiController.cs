using ApplicationCore.Entities;
using HRTechProject.Entities;
using IdentityDenemeHRTech.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Net.Mail;
using static System.Formats.Asn1.AsnWriter;

namespace IdentityDenemeHRTech.Controllers
{
    [Authorize(Roles = "SirketYoneticisi")]
    public class SirketYoneticisiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toast;

        public SirketYoneticisiController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, IToastNotification toast)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
            _toast = toast;
        }

        // Kullanıcı bilgilerini bütün sayfalarda göstermek için gerekli olan metot
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["Kullanici"] = _db.Users.Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .First(a => a.Id == _userManager.GetUserId(User));
            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AktifCalisanSayisi = _db.ApplicationUsers.Where(x => x.Aktiflik == true).Count();
            ViewBag.PasifCalisanSayisi = _db.ApplicationUsers.Where(x => x.Aktiflik == false).Count();
            ViewBag.DepartmanSayisi = _db.Departmanlar.Count();
            ViewBag.MeslekSayisi = _db.Meslekler.Count();
            ViewBag.YeniCalisanlar = _db.ApplicationUsers.Where(x => x.IseGirisTarihi > DateTime.Now.AddDays(-7) && x.Aktiflik == true).Count();
            ViewBag.OnaylananHarcamaTutari = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Onay).Sum(x => x.Tutar);
            ViewBag.RedHarcamaTutari = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Red).Sum(x => x.Tutar);
            ViewBag.BekleyenHarcamaTutari = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Bekleme).Sum(x => x.Tutar);

            if (TempData["personel"] != null)
            {
                ViewBag.p = "Personel Eklendi";
            }
            return View();
        }

        [HttpGet]
        public IActionResult SirketGuncelle()
        {
            var sirket = _db.Sirketler.Include(s => s.Departmanlar).FirstOrDefault();
            if (sirket == null)
            {
                return NotFound();
            }

            return View(sirket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SirketGuncelle(Sirket model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
        public async Task<IActionResult> Ozet()
        {
            var personel = _db.Users.Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .First(a => a.Id == _userManager.GetUserId(User));

            return View(personel);
        }


        public async Task<IActionResult> BekleyenAvanslar(int page = 1)
        {
            var list = _db.Avanslar.Where(x => x.Durum == ApplicationCore.Enums.AvansDurumu.Bekleme).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> OnaylananAvanslar(int page = 1)
        {
            var list = _db.Avanslar.Where(x => x.Durum == ApplicationCore.Enums.AvansDurumu.Onay).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> IptalAvanslar(int page = 1)
        {
            var list = _db.Avanslar.Where(x => x.Durum == ApplicationCore.Enums.AvansDurumu.Red).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> AvansRed(int id)
        {
            var redAvans = _db.Avanslar.FirstOrDefault(x => x.Id == id);

            if (redAvans != null)
            {
                redAvans.Durum = ApplicationCore.Enums.AvansDurumu.Red;
                await _db.SaveChangesAsync();
                _toast.AddErrorToastMessage("Avans Talebi Reddedildi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenAvanslar");
            }

            return NotFound();
        }
        public async Task<IActionResult> AvansOnay(int id)
        {
            var onayAvans = _db.Avanslar.FirstOrDefault(x => x.Id == id);

            if (onayAvans != null)
            {
                onayAvans.Durum = ApplicationCore.Enums.AvansDurumu.Onay;
                onayAvans.OnaylanmaTarihi = DateTime.Now;
                await _db.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Avans Talebi Onaylandı!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenAvanslar");
            }

            return NotFound();
        }
        public async Task<IActionResult> AvansBekleme(int id, string sender)
        {
            var beklemeAvans = _db.Avanslar.FirstOrDefault(x => x.Id == id);

            if (beklemeAvans != null)
            {
                beklemeAvans.Durum = ApplicationCore.Enums.AvansDurumu.Bekleme;
                if (beklemeAvans.OnaylanmaTarihi.HasValue)
                {
                    beklemeAvans.OnaylanmaTarihi = null;
                }
                await _db.SaveChangesAsync();
                _toast.AddWarningToastMessage("Avans Talebi Beklemeye Alındı!", new ToastrOptions { Title = "İşlem Başarılı" });
                if (sender == "onay")
                    return RedirectToAction("OnaylananAvanslar");
                else
                    return RedirectToAction("IptalAvanslar");
            }

            return NotFound();
        }

        public async Task<IActionResult> BekleyenIzinler(int page = 1)
        {
            var list = _db.Izinler.Where(x => x.Durumu == ApplicationCore.Enums.IzinDurumu.Bekleme).Include(x => x.User).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }
        public async Task<IActionResult> OnaylananIzinler(int page = 1)
        {
            var list = _db.Izinler.Where(x => x.Durumu == ApplicationCore.Enums.IzinDurumu.Onay).Include(x => x.User).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }
        public async Task<IActionResult> RedIzinler(int page = 1)
        {
            var list = _db.Izinler.Where(x => x.Durumu == ApplicationCore.Enums.IzinDurumu.Red).Include(x => x.User).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> IzinBekleme(int id, string sender)
        {
            var beklemeIzin = _db.Izinler.FirstOrDefault(x => x.Id == id);

            if (beklemeIzin != null)
            {
                beklemeIzin.Durumu = ApplicationCore.Enums.IzinDurumu.Bekleme;
                if (beklemeIzin.CevaplanmaTarihi.HasValue)
                {
                    beklemeIzin.CevaplanmaTarihi = null;
                }
                await _db.SaveChangesAsync();
                _toast.AddWarningToastMessage("İzin Talebi Beklemeye Alındı!", new ToastrOptions { Title = "İşlem Başarılı" });
                if (sender == "onay")
                    return RedirectToAction("OnaylananIzinler");
                else
                    return RedirectToAction("RedIzinler");
            }

            return NotFound();
        }
        public async Task<IActionResult> IzinOnay(int id)
        {
            var onayIzin = _db.Izinler.FirstOrDefault(x => x.Id == id);

            if (onayIzin != null)
            {
                onayIzin.Durumu = ApplicationCore.Enums.IzinDurumu.Onay;
                onayIzin.CevaplanmaTarihi = DateTime.Now;
                await _db.SaveChangesAsync();
                _toast.AddSuccessToastMessage("İzin Talebi Onaylandı!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenIzinler");
            }

            return NotFound();
        }
        public async Task<IActionResult> IzinRed(int id)
        {
            var onayIzin = _db.Izinler.FirstOrDefault(x => x.Id == id);

            if (onayIzin != null)
            {
                onayIzin.Durumu = ApplicationCore.Enums.IzinDurumu.Red;
                onayIzin.CevaplanmaTarihi = DateTime.Now;
                onayIzin.AktifMi = false;
                await _db.SaveChangesAsync();
                _toast.AddErrorToastMessage("İzin Talebi Reddedildi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenIzinler");
            }

            return NotFound();
        }
        public async Task<IActionResult> BekleyenHarcamalar(int page = 1)
        {
            var list = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Bekleme).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> OnaylananHarcamalar(int page = 1)
        {
            var list = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Onay).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> IptalHarcamalar(int page = 1)
        {
            var list = _db.Harcamalar.Where(x => x.Durum == ApplicationCore.Enums.IzinDurumu.Red).Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public async Task<IActionResult> HarcamaOnay(int id)
        {
            var onayHarcama = _db.Harcamalar.FirstOrDefault(x => x.Id == id);

            if (onayHarcama != null)
            {
                onayHarcama.Durum = ApplicationCore.Enums.IzinDurumu.Onay;
                onayHarcama.CevapTarihi = DateTime.Now;
                await _db.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Harcama Talebi Onaylandı!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenHarcamalar");
            }

            return NotFound();
        }
        public async Task<IActionResult> HarcamaRed(int id)
        {
            var redHarcama = _db.Harcamalar.FirstOrDefault(x => x.Id == id);

            if (redHarcama != null)
            {
                redHarcama.Durum = ApplicationCore.Enums.IzinDurumu.Red;
                redHarcama.CevapTarihi = DateTime.Now;
                await _db.SaveChangesAsync();
                _toast.AddErrorToastMessage("Harcama Talebi Reddedildi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("BekleyenHarcamalar");
            }

            return NotFound();
        }
        public async Task<IActionResult> HarcamaBekleme(int id, string sender)
        {
            var beklemeHarcama = _db.Harcamalar.FirstOrDefault(x => x.Id == id);

            if (beklemeHarcama != null)
            {
                beklemeHarcama.Durum = ApplicationCore.Enums.IzinDurumu.Bekleme;
                if (beklemeHarcama.CevapTarihi.HasValue)
                {
                    beklemeHarcama.CevapTarihi = null;
                }
                await _db.SaveChangesAsync();
                _toast.AddWarningToastMessage("Harcama Talebi Beklemeye Alındı!", new ToastrOptions { Title = "İşlem Başarılı" });
                if (sender == "onay")
                    return RedirectToAction("OnaylananHarcamalar");
                else
                    return RedirectToAction("IptalHarcamalar");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> PersonelEkle()
        {
            var model = new PersonelEkleViewModel()
            {
                Departmanlar = await _db.Departmanlar.ToListAsync(),
                IseGirisTarihi = DateTime.Now,
                DogumTarihi = new DateTime(2000, 1, 1),
                Sirketler = await _db.Sirketler.ToListAsync(),
                Meslekler = await _db.Meslekler.ToListAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PersonelEkle(PersonelEkleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var departman = await _db.Departmanlar.FirstOrDefaultAsync(x => x.Id == model.DepartmanId);
                var sirket = await _db.Sirketler.FirstOrDefaultAsync(x => x.Name == model.SirketAdi);
                var meslek = await _db.Meslekler.FirstOrDefaultAsync(x => x.Id == model.MeslekId);

                var adres = new Adres()
                {
                    DaireNo = model.Adres.DaireNo,
                    Il = model.Adres.Il,
                    Ilce = model.Adres.Ilce,
                    KapiNo = model.Adres.KapiNo,
                    Mahalle = model.Adres.Mahalle,
                    Sokak = model.Adres.Sokak
                };
                _db.Adresler.Add(adres);
                _db.SaveChanges();

                var calisan = new ApplicationUser()
                {
                    UserName = model.Mail,
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Maas = model.Maas,
                    Email = model.Mail,
                    EmailConfirmed = true,
                    IseGirisTarihi = model.IseGirisTarihi,
                    Cinsiyet = model.Cinsiyet,
                    Telefon = model.Telefon,
                    DepartmanId = departman.Id,
                    AdresId = adres.Id,
                    SirketId = sirket.Id,
                    MeslekId = meslek.Id,
                    DogumTarihi = model.DogumTarihi,
                    PhoneNumber = model.Telefon,
                    TcNo = model.TC,
                    IstenCikisTarihi = null,
                    Aktiflik = true,
                    Harcamalar = new List<Harcama>(),
                    Izinler = new List<Izin>(),
                    Avanslar = new List<Avans>(),
                };

                if (model.Fotograf != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/personel", model.Fotograf.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.Fotograf.CopyToAsync(stream);
                    }

                    calisan.FotografYolu = model.Fotograf.FileName;
                }

                var result = await _userManager.CreateAsync(calisan, model.Sifre);                
                if (result.Succeeded)
                {
                    TempData["personel"] = "eklendi";

                    //MAIL BÖLÜMÜ

                    MailMessage message = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();

                    MailViewModel vm = new MailViewModel()
                    {
                        Sender = "hrtechonline6@gmail.com",
                        Reciever = $"{model.Mail}"
                    };

                    message.From = new MailAddress(vm.Sender);
                    message.To.Add(new MailAddress(vm.Reciever));
                    message.Subject = "Personel HRTECH Girişi Hk.";
                    message.Body = $"İlk kez giriş yaparken {model.Sifre} şifresini kullanabilirsiniz. Lütfen giriş yaptıktan sonra şifrenizi değiştirin.";

                    smtpClient.Port = 587;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.EnableSsl = true;

                    smtpClient.Credentials = new NetworkCredential(vm.Sender, "ygkictvgvolqbnwe");

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtpClient.Send(message);

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // Departmanlar, Sirketler ve meslekler listelerini yeniden doldurun
            model.Departmanlar = await _db.Departmanlar.ToListAsync();
            model.Sirketler = await _db.Sirketler.ToListAsync();
            model.Meslekler = await _db.Meslekler.ToListAsync();            

            return View(model);
        }

    }
}