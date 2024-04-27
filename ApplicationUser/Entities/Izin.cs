using ApplicationCore.Attributes.Personel;
using ApplicationCore.Enums;
using HRTechProject.Entities;

namespace ApplicationCore.Entities
{

    public class Izin : BaseEntity
    {
        public IzinTuru Turu { get; set; }

        [IzinEskiTarihKontrol]
        public DateTime BaslangicTarihi { get; set; }
        
        public DateTime BitisTarihi { get; set; }

        public bool? AktifMi { get; set; }

        public int GunSayisi => HesaplaHaftaIciGunSayisi();
        public IzinDurumu Durumu { get; set; }
        public DateTime? CevaplanmaTarihi { get; set; }          
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? User { get; set; }        
        public Izin()
        {
            Durumu = IzinDurumu.Bekleme;
        }

        private int HesaplaHaftaIciGunSayisi()
        {
            TimeSpan fark = BitisTarihi - BaslangicTarihi;
            int toplamGun = fark.Days + 1;
            int haftaIciGunSayisi = 0;

            for (int i = 0; i < toplamGun; i++)
            {
                DateTime gun = BaslangicTarihi.AddDays(i);
                if (gun.DayOfWeek != DayOfWeek.Saturday && gun.DayOfWeek != DayOfWeek.Sunday)
                {
                    haftaIciGunSayisi++;
                }
            }

            return haftaIciGunSayisi;
        }        

        public bool GunSayisiKisitlamasi(out string errorMessage)
        {
            errorMessage = "";

            switch (Turu)
            {
                case IzinTuru.Babalik:
                    if (GunSayisi > 5)
                    {
                        errorMessage = "Babalık izni en fazla 5 gün olabilir.";
                        return false;
                    }
                    break;
                case IzinTuru.Hastalik:
                    if (GunSayisi > 10)
                    {
                        errorMessage = "Hastalık izni en fazla 10 gün olabilir.";
                        return false;
                    }
                    break;
                case IzinTuru.Hamilelik:
                    if (GunSayisi > 80)
                    {
                        errorMessage = "Hamilelik izni en fazla 80 gün olabilir.";
                        return false;
                    }
                    break;
                case IzinTuru.Olum:
                    if (GunSayisi > 3)
                    {
                        errorMessage = "Ölüm izni en fazla 3 gün olabilir.";
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
