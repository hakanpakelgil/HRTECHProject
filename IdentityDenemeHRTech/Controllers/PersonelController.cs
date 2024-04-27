using ApplicationCore.Entities;
using ApplicationCore.Enums;
using HRTechProject.Entities;
using IdentityDenemeHRTech.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Collections.Generic;

namespace HRTechProject.Controllers
{
    public class PersonelController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toast;

        public PersonelController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, IToastNotification toast)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
            _toast = toast;
        }
        public async Task<IActionResult> Index()
        {
            var personel = _db.Users.Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .First(a => a.Id == _userManager.GetUserId(User));
            return View(personel);
        }

        public async Task<IActionResult> Details()
        {
            if (TempData.ContainsKey("GuncelleMesaji"))
            {
                ViewBag.GuncelleMesaji = TempData["GuncelleMesaji"];
            }
            var personel = _db.Users.Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .First(a => a.Id == _userManager.GetUserId(User));
            List<Izin> kullanilanIzinler = _db.Izinler.Where(x => x.ApplicationUserId == _userManager.GetUserId(User)).ToList();
            int kullanilanIzinGunSayisi = kullanilanIzinler.Sum(x => x.GunSayisi);
            ViewBag.IzinGunSayisi = personel.IzinHakki - kullanilanIzinGunSayisi;
            return View(personel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var personel = _db.Users.Include(x => x.Adres)
                 .Include(x => x.Departman)
                 .Include(x => x.Meslek)
                 .Include(x => x.Adres)
                 .First(a => a.Id == _userManager.GetUserId(User));

            var viewModel = new PersonelGuncellemeViewModel
            {
                Ad = personel.Ad,
                Soyad = personel.Soyad,
                Meslek = personel.Meslek,
                Departman = personel.Departman,
                TcNo = personel.TcNo,
                DogumTarihi = personel.DogumTarihi,
                Maas = personel.Maas,
                IseGirisTarihi = personel.IseGirisTarihi,
                IstenCikisTarihi = personel.IstenCikisTarihi,
                FotografYolu = personel.FotografYolu,
                Adres = personel.Adres,
                Il = personel.Adres.Il,
                Ilce = personel.Adres.Ilce,
                Mahalle = personel.Adres.Mahalle,
                Sokak = personel.Adres.Sokak,
                KapiNo = personel.Adres.KapiNo,
                DaireNo = personel.Adres.DaireNo,
                Telefon = personel.Telefon
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonelGuncellemeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var personel = _db.Users.Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .Include(x => x.Adres)
                .First(a => a.Id == _userManager.GetUserId(User));
            if (personel == null)
            {
                return NotFound();
            }

            // Mevcut fotoğraf yolu
            var eskiFotografYolu = personel.FotografYolu;

            // fotoğraf yükleme işlemi
            if (vm.Fotograf != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/personel", vm.Fotograf.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await vm.Fotograf.CopyToAsync(stream);
                }

                personel.FotografYolu = vm.Fotograf.FileName;
            }
           

            personel.Telefon = vm.Telefon;
            personel.Adres.Il = vm.Il;
            personel.Adres.Ilce = vm.Ilce;
            personel.Adres.Mahalle = vm.Mahalle;
            personel.Adres.Sokak = vm.Sokak;
            personel.Adres.KapiNo = vm.KapiNo;
            personel.Adres.DaireNo = vm.DaireNo;


            var result = await _userManager.UpdateAsync(personel);
            if (result.Succeeded)
            {
                // Eğer güncelleme başarılı olduysa, eski resim dosyasını sil
                if (vm.Fotograf != null && !string.IsNullOrEmpty(eskiFotografYolu))
                {
                    var eskiFotografYoluTam = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/personel", eskiFotografYolu);
                    if (System.IO.File.Exists(eskiFotografYoluTam))
                    {
                        System.IO.File.Delete(eskiFotografYoluTam);
                    }
                }

                _toast.AddSuccessToastMessage("Personel Bilgileri Güncellendi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Details");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }


        public async Task<IActionResult> Izin(int page = 1)
        {
            var userId = _userManager.GetUserId(User);

            var personel = await _db.Users
                .Include(x => x.Adres)
                .Include(x => x.Departman)
                .Include(x => x.Meslek)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (personel == null)
            {
                return NotFound();
            }

            var izinler = await _db.Izinler
                .Include(x => x.User)
                .OrderByDescending(x => x.Id)
                .Where(x => x.ApplicationUserId == userId)
                .ToListAsync();

            int buSenekiKullanilanIzinGunSayisi = izinler
                .Where(x => x.Turu == IzinTuru.Diger && x.Durumu == IzinDurumu.Onay && x.BitisTarihi.Year == DateTime.Now.Year)
                .Sum(x => x.GunSayisi);

            int buSenekiKalanYillikIzinGunSayisi = 14 - buSenekiKullanilanIzinGunSayisi;

            int gecenSenedenKalanYillikIzinGunSayisi = 0;

            int toplamYillikIzinGunSayisi = buSenekiKalanYillikIzinGunSayisi + gecenSenedenKalanYillikIzinGunSayisi;

            if (buSenekiKalanYillikIzinGunSayisi < 0)
            {
                gecenSenedenKalanYillikIzinGunSayisi = Math.Abs(buSenekiKalanYillikIzinGunSayisi);
                buSenekiKalanYillikIzinGunSayisi = 0;
            }

            ViewBag.ToplamYillikIzinHakki = toplamYillikIzinGunSayisi;
            ViewBag.BulundugumuzYildanKalanYillikIzinHakki = buSenekiKalanYillikIzinGunSayisi;
            ViewBag.KullanilanYillikIzinHakki = buSenekiKullanilanIzinGunSayisi;
            ViewBag.GecenSenedenKalanYillikIzinHakki = gecenSenedenKalanYillikIzinGunSayisi;

            return View(izinler);
        }

        [HttpGet]
        public IActionResult IzinEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IzinEkle(Izin yeniIzin)
        {
            if (ModelState.IsValid)
            {
                if (yeniIzin.BaslangicTarihi > yeniIzin.BitisTarihi)
                {
                    ModelState.AddModelError(string.Empty, "Başlangıç tarihi bitiş tarihinden sonra olamaz!");
                    return View(yeniIzin);
                }
                string errorMessage = "";
                if (!yeniIzin.GunSayisiKisitlamasi(out errorMessage))
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(yeniIzin);
                }

                var userId = _userManager.GetUserId(User);
                var personel = await _db.Users
                    .Include(x => x.Izinler)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (yeniIzin.Turu == ApplicationCore.Enums.IzinTuru.Babalik && personel.Cinsiyet == ApplicationCore.Enums.Cinsiyet.Kadin)
                {
                    ModelState.AddModelError(string.Empty, "Kadın personel babalık iznini seçemez!");
                    return View(yeniIzin);
                }
                if (yeniIzin.Turu == ApplicationCore.Enums.IzinTuru.Hamilelik && personel.Cinsiyet == ApplicationCore.Enums.Cinsiyet.Erkek)
                {
                    ModelState.AddModelError(string.Empty, "Erkek personel hamilelik iznini seçemez!");
                    return View(yeniIzin);
                }

                yeniIzin.Durumu = ApplicationCore.Enums.IzinDurumu.Bekleme;
                yeniIzin.AktifMi = true;

                personel.Izinler.Add(yeniIzin);
                await _db.SaveChangesAsync();
                _toast.AddSuccessToastMessage("İzin Talebi Oluşturuldu!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Izin");
            }
            return View(yeniIzin);
        }
        [HttpGet]
        public async Task<IActionResult> IzınIptal(int id)
        {
            var userId = _userManager.GetUserId(User);
            var izin = await _db.Users
       .Where(u => u.Id == userId)
       .SelectMany(u => u.Izinler)
       .FirstOrDefaultAsync(i => i.Id == id);
            return View(izin);
        }
        [HttpPost]
        public async Task<IActionResult> IzınIptal(Izin model)
        {
            var izin = await _db.Izinler.FindAsync(model.Id);
            if (izin == null)
            {
                return NotFound();
            }
            izin.AktifMi = false;
            await _db.SaveChangesAsync();
            _toast.AddSuccessToastMessage("İzin Talebi Silindi!", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("Izin");
        }

        public IActionResult Harcamalarim(int page = 1)
        {
            var userId = _userManager.GetUserId(User);

            var list = _db.Harcamalar.Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).Where(x => x.ApplicationUserId == userId).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        public IActionResult HarcamaEkle()
        {
            return View();

        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> HarcamaEkle(YeniHarcamaViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string ext = Path.GetExtension(vm.Dosya.FileName);
                string yeniDosyaAd = Guid.NewGuid() + ext;
                string yol = Path.Combine(_env.WebRootPath, "upload-files", "personel", yeniDosyaAd);

                using (var fs = new FileStream(yol, FileMode.CreateNew))
                {
                    vm.Dosya.CopyTo(fs);
                }

                await _db.Harcamalar.AddAsync(new Harcama
                {
                    Tur = vm.Tur,
                    Tutar = vm.Tutar,
                    ParaBirimi = vm.ParaBirimi,
                    Dosya = yeniDosyaAd,
                    Durum = ApplicationCore.Enums.IzinDurumu.Bekleme,
                    TalepTarihi = DateTime.Now,
                    CevapTarihi = null,
                    ApplicationUserId = _userManager.GetUserId(User),
                    //deneme
                });
                _db.SaveChanges();
                _toast.AddSuccessToastMessage("Harcama Talebi Oluşturuldu!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Harcamalarim");
            }
            ViewBag.Validasyon = "düzenle";
            return View(vm);
        }

        public IActionResult Duzenle(int id)
        {

            var harcama = _db.Harcamalar.FirstOrDefault(x => x.Id == id);
            if (harcama == null)
            {
                return NotFound();
            }
            var harcamaVM = new HarcamaDuzenleViewModel();
            harcamaVM.Durum = harcama.Durum;
            harcamaVM.ParaBirimi = harcama.ParaBirimi;
            harcamaVM.TalepTarihi = harcama.TalepTarihi;
            harcamaVM.Tur = harcama.Tur;
            harcamaVM.Tutar = harcama.Tutar;
            return View(harcamaVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Duzenle(HarcamaDuzenleViewModel harcamaVM)
        {
            if (ModelState.IsValid)
            {
                var mevcutHarcama = _db.Harcamalar.FirstOrDefault(h => h.Id == harcamaVM.Id);
                if (mevcutHarcama != null)
                {
                    if (harcamaVM.Dosya != null && harcamaVM.Dosya.Length > 0)
                    {
                        string ext = Path.GetExtension(harcamaVM.Dosya.FileName);
                        string yeniDosyaAd = Guid.NewGuid() + ext;
                        string yol = Path.Combine(_env.WebRootPath, "upload-files", "personel", yeniDosyaAd);
                        using (var fs = new FileStream(yol, FileMode.CreateNew))
                        {
                            harcamaVM.Dosya.CopyTo(fs);
                        }
                        mevcutHarcama.Dosya = yeniDosyaAd;
                    }
                    mevcutHarcama.Tur = harcamaVM.Tur;
                    mevcutHarcama.Tutar = harcamaVM.Tutar;
                    mevcutHarcama.ParaBirimi = harcamaVM.ParaBirimi;
                    mevcutHarcama.Durum = ApplicationCore.Enums.IzinDurumu.Bekleme;
                    mevcutHarcama.TalepTarihi = DateTime.Now;
                    _db.SaveChanges();
                    _toast.AddSuccessToastMessage("Harcama Talebi Güncellendi!", new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Harcamalarim");
                }
                else
                    return NotFound();
            }
            TempData["Duzenle"] = "düzenle";
            return View(harcamaVM);
        }
        public IActionResult Sil(int id)
        {
            var harcama = _db.Harcamalar.FirstOrDefault(x => x.Id == id);
            if (harcama == null)
            {
                return NotFound();
            }
            var harcamaVM = new HarcamaDuzenleViewModel();
            harcamaVM.Durum = harcama.Durum;
            harcamaVM.ParaBirimi = harcama.ParaBirimi;
            harcamaVM.TalepTarihi = harcama.TalepTarihi;
            harcamaVM.Tur = harcama.Tur;
            harcamaVM.Tutar = harcama.Tutar;
            return View(harcamaVM);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Sil(HarcamaDuzenleViewModel harcamaVM)
        {
            var mevcutHarcama = _db.Harcamalar.FirstOrDefault(h => h.Id == harcamaVM.Id);
            if (mevcutHarcama != null)
            {
                if (harcamaVM.Dosya != null)
                    mevcutHarcama.Dosya = harcamaVM.Dosya.FileName;
                mevcutHarcama.Tur = harcamaVM.Tur;
                mevcutHarcama.Tutar = harcamaVM.Tutar;
                mevcutHarcama.ParaBirimi = harcamaVM.ParaBirimi;
                mevcutHarcama.Durum = ApplicationCore.Enums.IzinDurumu.Bekleme;
                mevcutHarcama.TalepTarihi = DateTime.Now;
                _db.Harcamalar.Remove(mevcutHarcama);
                _db.SaveChanges();
                _toast.AddSuccessToastMessage("Harcama Talebi Silindi!", new ToastrOptions { Title = "İşlem Başarılı" });
            }
            return RedirectToAction("Harcamalarim");
        }

        public async Task<IActionResult> Avans(int page = 1)
        {
            var userId = _userManager.GetUserId(User);

            var list = _db.Avanslar.Include(x => x.ApplicationUser).OrderByDescending(x => x.Id).Where(x => x.ApplicationUserId == userId).ToList();

            var pagedItemList = list.Skip((page - 1) * 15).Take(15);

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = Math.Ceiling((double)list.Count / 15);

            return View(pagedItemList.ToList());
        }

        [HttpGet]
        public IActionResult AvansEkle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AvansEkle(YeniAvansViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var personel = await _db.Users
                    .Include(x => x.Avanslar)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (personel == null)
                {
                    return NotFound();
                }

                var birYilOnce = DateTime.Now.AddYears(-1);
                var toplamAvansTutariBirYilIcinde = personel.Avanslar
                    .Where(a => a.Tarih >= birYilOnce && (a.Durum == AvansDurumu.Onay || a.Durum == AvansDurumu.Bekleme))
                    .Sum(a => a.Tutar);

                var maksimumAvansMiktariBirYilIcinde = personel.Maas * 3;
                if (toplamAvansTutariBirYilIcinde + vm.Tutar > maksimumAvansMiktariBirYilIcinde)
                {
                    ModelState.AddModelError(string.Empty, "1 yıl içinde en fazla 3 maaşlık avans talep edebilirsiniz.");
                    return View(vm);
                }

                if (vm.Tutar > personel.Maas)
                {
                    ModelState.AddModelError(string.Empty, "Tek seferde en fazla maaş kadar avans talep edebilirsiniz.");
                    return View(vm);
                }

                await _db.Avanslar.AddAsync(new Avans
                {
                    Tutar = vm.Tutar,
                    Tur = vm.Tur,
                    Aciklama = vm.Aciklama,
                    Tarih = DateTime.Now,
                    Durum = AvansDurumu.Bekleme,
                    ApplicationUserId = userId
                });
                await _db.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Avans Talebi Oluşturuldu!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Avans");
            }

            return View(vm);
        }

        public IActionResult AvansDuzenle(int id)
        {
            var avans = _db.Avanslar.FirstOrDefault(x => x.Id == id);

            if (avans == null)
            {
                return NotFound();
            }

            var avansVM = new AvansDuzenleViewModel();
            avansVM.Id = avans.Id;
            avansVM.Tutar = avans.Tutar;
            avansVM.Tur = avans.Tur;
            avansVM.Aciklama = avans.Aciklama;

            return View(avansVM);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AvansDuzenle(AvansDuzenleViewModel avansVM)
        {
            if (ModelState.IsValid)
            {

                var mevcutAvans = _db.Avanslar.FirstOrDefault(h => h.Id == avansVM.Id);

                if (mevcutAvans == null)
                {
                    return NotFound();
                }

                mevcutAvans.Tutar = avansVM.Tutar;
                mevcutAvans.Tur = avansVM.Tur;
                mevcutAvans.Aciklama = avansVM.Aciklama;

                _db.SaveChanges();
                _toast.AddSuccessToastMessage("Avans Talebi Güncellendi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Avans");
            }

            return View(avansVM);
        }
        public IActionResult AvansIptalEt(int id)
        {
            var avans = _db.Avanslar.FirstOrDefault(x => x.Id == id);

            if (avans == null)
            {
                return NotFound();
            }
            var avansVM = new AvansSilViewModel();
            avansVM.Id = avans.Id;
            avansVM.Tutar = avans.Tutar;
            avansVM.Tur = avans.Tur;
            avansVM.Aciklama = avans.Aciklama;

            return View(avansVM);
        }
        [HttpPost]
        public IActionResult AvansIptalEt(AvansSilViewModel model)
        {
            if (ModelState.IsValid)
            {
                var avans = _db.Avanslar.FirstOrDefault(x => x.Id == model.Id);
                if (avans == null)
                {
                    return NotFound();
                }
                _db.Avanslar.Remove(avans);
                _db.SaveChanges();
                _toast.AddSuccessToastMessage("Avans Talebi Silindi!", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Avans");
            }
            return View(model);
        }
    }
}

