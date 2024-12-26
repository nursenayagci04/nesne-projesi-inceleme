using System;
using System.Collections.Generic;

namespace CalisanTakip
{
    // Çalışan bilgileri için temel sınıf
    public class Calisan
    {
        private string ad;
        private string soyad;
        private int yas;
        private string departman;
        private string pozisyon;
        private int izinGunleri;
        private string tcKimlikNo;
        private string sifre;

        public string Ad => ad;
        public string Soyad => soyad;
        public int Yas => yas;
        public string Departman => departman;
        public string Pozisyon => pozisyon;

        public Calisan(string ad, string soyad, int yas, string departman, string pozisyon, string tcKimlikNo, string sifre)
        {
            if (yas <= 0)
            {
                Console.WriteLine("Hata: Yaş 0 veya negatif olamaz.");
                return;
            }

            this.ad = ad;
            this.soyad = soyad;
            this.yas = yas;
            this.departman = departman;
            this.pozisyon = pozisyon;
            this.tcKimlikNo = tcKimlikNo;
            this.sifre = sifre;
            this.izinGunleri = 0; // Başlangıçta izin günleri 0
        }

        public void IzinGunleriEkle(int gun)
        {
            if (gun < 0)
            {
                Console.WriteLine("Hata: Gün sayısı negatif olamaz.");
                return;
            }

            izinGunleri += gun;
        }

        public int IzinGunleriGetir() => izinGunleri;

        public string TcKimlikNoGoster() => tcKimlikNo;

        public bool SifreDogrula(string girilenSifre) => sifre == girilenSifre;

        public override string ToString()
        {
            return "Ad: " + Ad + " " + Soyad + ", Yaş: " + Yas + ", Departman: " + Departman + ", Pozisyon: " + Pozisyon + ", İzin Günleri: " + izinGunleri;
        }

        public string DetayliBilgiGoster()
        {
            return "TC Kimlik No: " + tcKimlikNo + ", Şifre: " + sifre;
        }
    }

    public class CalisanYonetici
    {
        private List<Calisan> calisanlar = new List<Calisan>();

        public void CalisanEkle(Calisan calisan)
        {
            if (calisan == null)
            {
                Console.WriteLine("Hata: Geçersiz çalışan nesnesi.");
                return;
            }

            calisanlar.Add(calisan);
        }

        public void GirisYap(string ad, string soyad, string sifre)
        {
            foreach (var calisan in calisanlar)
            {
                if (calisan.Ad == ad && calisan.Soyad == soyad)
                {
                    if (calisan.SifreDogrula(sifre))
                    {
                        Console.WriteLine("Giriş başarılı!");
                        Console.WriteLine(calisan);

                        Console.Write("Detaylı bilgi görmek istiyor musunuz? (evet/hayır): ");
                        string cevap = Console.ReadLine();

                        if (cevap.ToLower() == "evet")
                        {
                            Console.WriteLine("Detaylı Bilgi: " + calisan.DetayliBilgiGoster());
                        }
                        else
                        {
                            Console.WriteLine("Detaylı bilgi gösterilmedi.");
                        }
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Hata: Şifre yanlış.");
                        return;
                    }
                }
            }

            Console.WriteLine("Hata: Çalışan bulunamadı.");
        }

        public void CalisanlariListele()
        {
            if (calisanlar.Count == 0)
            {
                Console.WriteLine("Hiç çalışan bulunamadı.");
                return;
            }

            foreach (var calisan in calisanlar)
            {
                Console.WriteLine(calisan);
            }
        }

        public void IzinGunleriGuncelle(string ad, string soyad, int yeniIzinGunleri)
        {
            foreach (var calisan in calisanlar)
            {
                if (calisan.Ad == ad && calisan.Soyad == soyad)
                {
                    calisan.IzinGunleriEkle(yeniIzinGunleri);
                    Console.WriteLine($"İzin günleri güncellendi. Yeni izin günleri: {calisan.IzinGunleriGetir()}");
                    return;
                }
            }
            Console.WriteLine("Çalışan bulunamadı.");
        }

        public void IzinKullan(string ad, string soyad, int kullanilanGun)
        {
            foreach (var calisan in calisanlar)
            {
                if (calisan.Ad == ad && calisan.Soyad == soyad)
                {
                    if (calisan.IzinGunleriGetir() >= kullanilanGun)
                    {
                        calisan.IzinGunleriEkle(-kullanilanGun);
                        Console.WriteLine($"{kullanilanGun} gün izin kullandınız. Kalan izin gününüz: {calisan.IzinGunleriGetir()}");
                    }
                    else
                    {
                        Console.WriteLine("Yetersiz izin günü!");
                    }
                    return;
                }
            }
            Console.WriteLine("Çalışan bulunamadı.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CalisanYonetici yonetici = new CalisanYonetici();

            // Çalışan ekleme
            var calisan1 = new Calisan("Nehir", "Saygılı", 20, "Mühendislik", "Kıdemli Mühendis", "12345678901", "333");
            var calisan2 = new Calisan("Nursena", "Yağcı", 20, "Pazarlama", "Uzman", "98765432109", "7777");

            yonetici.CalisanEkle(calisan1);
            yonetici.CalisanEkle(calisan2);

            // İzin günleri ekleyelim
            yonetici.IzinGunleriGuncelle("Nehir", "Saygılı", 15); // Nehir'e 15 gün izin ekleyelim
            yonetici.IzinGunleriGuncelle("Nursena", "Yağcı", 10); // Nursena'ya 10 gün izin ekleyelim

            // Şifre kontrolüyle giriş
            Console.WriteLine("Giriş yapmak için adınızı, soyadınızı ve şifrenizi giriniz:");
            Console.Write("Ad: ");
            string ad = Console.ReadLine();
            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();
            Console.Write("Şifre: ");
            string sifre = Console.ReadLine();

            yonetici.GirisYap(ad, soyad, sifre);

            // İzin kullanma işlemi
            Console.WriteLine("\nİzin kullanmak için adınızı, soyadınızı ve kullanmak istediğiniz izin gününü giriniz:");
            Console.Write("Ad: ");
            ad = Console.ReadLine();
            Console.Write("Soyad: ");
            soyad = Console.ReadLine();
            Console.Write("Kullanılacak İzin Gün Sayısı: ");
            int izinGunu = int.Parse(Console.ReadLine());

            yonetici.IzinKullan(ad, soyad, izinGunu);
        }
    }
}
