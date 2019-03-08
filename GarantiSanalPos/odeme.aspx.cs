using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Text;

namespace GarantiSanalPos
{
    public partial class odeme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["status"] == "error")// ÇEKİM İŞLEMİ BAŞARISIZ OLURSA  BU İF İÇERİSİNE GİRER
                {

                    //foreach (var item in Request.)
                    //{

                    //}
                    //  Response.Write(Request.para)
                    //BANKADAN OLUMSUZ YANIT DÖNERSE BURAYA GİRER
                }
                else if (Request.QueryString["status"] == "success") // ÇEKİM İŞLEMİ BAŞARILI OLURSA BU İF İÇERİSİNE GİRER
                {
                    //AŞAĞIDAKİ  KOD SATIRLARI  PARAMETRELERİ GÖRMEK İÇİN ÖRNEK OLARAK YAZILMIŞTIR. ONLARIN YERİNE BAŞARILI ÖDEME SONRASI YAPILACAK İŞLEMLER YAZILABİLİR.
                    var Token = Request.Form.Get("Token");
                    string YanitParams = "";
                    for (int i = 0; i < Request.Params.Count; i++)
                    {
                        if (Request.Params.GetKey(i) == "ALL_HTTP")
                        {
                            break;
                        }
                        YanitParams += Request.Params.GetKey(i) + "= " + Request.Params.Get(i) + Environment.NewLine;
                    }
                    Response.Write("<pre>" + YanitParams + "</pre>");
                }
            }
            catch (Exception)
            {


            }

        }

        protected void btnOdemeYap_Click(object sender, EventArgs e)
        {
            OdemeYap();
        }

        /// <summary>
        /// BU FONKSİYON  HASH İŞLEMİ İÇİN KULLANILIYOR İÇERİĞİ DEĞİŞTİRİLMEYECEK
        /// </summary>
        /// <param name="SHA1Data"></param>
        /// <returns></returns>
        public string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider(); string HashedPassword = SHA1Data; byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword); byte[] inputbytes = sha.ComputeHash(hashbytes); return GetHexaDecimal(inputbytes);
        }
        /// <summary>
        /// BU FONKSİYON  HASH İŞLEMİ İÇİN KULLANILIYOR İÇERİĞİ DEĞİŞTİRİLMEYECEK
        /// </summary>
        /// <param name="SHA1Data"></param>
        /// <returns></returns>
        public string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder(); int length = bytes.Length; for (int n = 0; n <= length - 1; n++) { s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0")); }
            return s.ToString();
        }
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// BU FONKSİYONU  BEN RASTGELE SATIŞ NUMARASI VE TOKEN OLUŞTURMAK İÇİN YAPTIM. KULLANILMAK ZORUNDA DEĞİL
        /// </summary>
        /// <returns></returns>
        public string RastgeleKelime()
        {
            string kelime = "";
            var rnd = new Random();
            for (int i = 0; i < 12; i++)
            {
                kelime += ((char)rnd.Next('A', 'Z')).ToString();
            }
            return kelime;
        }
        void OdemeYap()
        {

            //GENEL İŞLEYİŞ ANLATIM
            /* 
             AŞAĞIDAKİ BİLGİLER İLE  POST EDİLECEK. 

             SUCCESURL PARAMETRESİNİN SONUNA TOKEN DEĞERİ EKLEDİM RASTGELE OLUŞUYOR. EĞER İŞLEM BAŞARILI İSE PAGE LOADDA
             SUCCES İLE BİRLİKTE TOKEN GERİ GELECEK.  BU TOKEN DEĞERİ YERİNE  SİPARŞİ NUMARASI GİRİLEBİLİR BÖYLELİKLE BANKADAN
             GELEN SUCCESS YANITI İLE BİRLİKTE  TOKEN DEĞERİ İÇERİSİNDE GELEN SİPARİŞNO İLE  ÖDEMENİN HANGİ SİPARİŞE AİT OLDUĞU
             BULUNUP GEREKLİ SİPARİŞ TAMAMLAMA İŞLEMLERİ YAPILABİLİR.

            NORMALDE DİĞER BANKALARDA TEST ESNASINDA DA MOBİL ONAY EKRANI GELİP BANKA TEST  MOBİL ONAY ŞİFRESİ VERİR O GİRİLEREK DEVAM EDER
            AMA GARANTİ BANKASI TEST ORTAMINDA MOBİL ONAY EKRANINI GETİRMEK YERİNE  EĞER KART BİLGİLERİ DOĞRUYSA MOBİL ONAY YAPILMIŞ GİBİ
            DİREKT OLARAK İŞLEMİ BAŞARILI SAYIYOR.  

            AŞAĞIDAKİ TEST ORTAMI BİLGİLERİ CANLI ORTAM BİLGİLERİ İLE DEĞİŞTİRİLDİĞİNDE  ÇEKİM İŞLEMİNDE OTOMATİK OLARAK MOBİL ONAY EKRANI DA
            GELECEKTİR. SONRASINDA ÇEKİMİ SMS ŞİFRESİ İLE YAPAR
             */
            string Token = RastgeleKelime();

            string posturl = "https://sanalposprovtest.garanti.com.tr/servlet/gt3dengine";// TEST ORTAMINDA BU URLYE POST EDİLECEK
                                                                                          //  string posturl= "https://sanalposprov.garanti.com.tr/servlet/gt3dengine";// CANLIYA ALINDIĞINDA BU URLYE POST EDİLECEK
                                                                                          /////////////////////KART BİLGİLERİ
            string KartNo = txtKartNo.Text; //Çekim yapılacak kredi kartı numarası
            string KartYil = txtYil.Text; // çekim yapılan kart üzerinde yazan yıl değeri (2 haneli olacak 2019 için 19 yazılacak);
            string KartAy = txtAy.Text;//Çekim yapılan kredi kartı üzerinde yazan ay değeri (2 haneli olacak 2. ay için 02 yazılacak);
            string Cvv = txtCvv.Text; //kredi kartı arkasında yazan cvv değeri
            /////////////////////////////////

            string Mode = "PROD";//değiştirilmeyecek
            string ApiVersion = "v0.01";//değiştirilmeyecek

            ////////////////////işyerine ait bilgiler

            //şuan test bilgilerine göre ayarlı. Canlıya almak için işyerine banka tarafından verilen bilgiler ile dğeiştirilecek.
            string TerminalId = "30691298";//İşyeri TErminal Id 
            string TerminalId_ = "030691298"; //üye işyeri terminal id ama burası 9 haneli olacak  ondan dolayı başına 0 ekliyoruz
            string TerminalMerchantId = "7000679";//işyeri numarası
            string TerminalUserId = "PROVAUT";//terminal userid- genelde sabit provaut oluyor değiştirilmeye gereko lmuyor ama banka farklı verebilir
            string TerminalProvUserId = "PROVAUT";//yukarıdaki terminaluserıdnin aynısı
            string ProvisionPassword = "123qweASD/"; // bankadan alınacak olan  sanalpos provision şifresi
            string StoreKey = "12345678"; //garanti banka panelinden oluşturulacak olan key.
            //////////////////////////////////////////

            /////////////////////////SATIŞA AİT BİLGİLER
            string TxnType = "sales";//değiştirilmeyecek
            string TxnAmount = "500";// ödeme tutarı (kuruş ayrımı koyulmayacak. 2 tl 30 kuruşluk ödeme yapılacaksa 2,30 değil 230 yazılacak 2tllik ödeme yapılacaksa 2 değil 200 yazılacak son iki rakam kuruş için)
            string OrderId = RastgeleKelime();// satış numarası. (Her ödeme için farklı oluşturulması gerek. örneğin: sepet idsi+saattarihbilgisi+userid gibi bir şey olabilir)
            string InstallmentCount = "2";// TAKSİT SAYISI EĞER TAKSİT İŞLEMİ YAPILMAYACAKSA  İÇERİK BOŞ OLACAK

            /////////////////////////////////////////


            ///////////////////////////////sanalpos diğer bilgiler
            string CurrencyCode = "949";//tl için 949 olması gerekli 
            string SuccessUrl = "http://localhost:53513/odeme.aspx?status=success&token=" + Token;//Ödeme işlemi başarılı ise yönlendirilecek olan url
            string ErrorUrl = "http://localhost:53513/odeme.aspx?status=error";//Ödeme işlemi başarısız ise yönlendirilecek url
            string CustomerIpAdress = "127.0.0.1";// Müşteri Ip adresi
            string Secure3dHash = ""; //BU KISIM AŞAĞIDAKİ HASH İŞLEMİNDE DOLDURULUYOR
            string Secure3dSecurityLevel = "3D_PAY";//DEĞİŞTİRİLMEYECEK
                                                    ////////////////////////////////////////////

            ////////////////////BU ALAN SABİT OLACAK DEĞİŞTİRİLMEYECEK !!
            SHA1 sha = new SHA1CryptoServiceProvider();
            var securitydata = GetSHA1(ProvisionPassword + TerminalId_).ToUpper(); //GÜVENLİK DOĞRULAMASI İÇİN STANDART oluşturulacak hash

            var hashdata = TerminalId + OrderId + TxnAmount + SuccessUrl + ErrorUrl + TxnType + InstallmentCount + StoreKey + securitydata;
            Secure3dHash = GetSHA1(hashdata).ToUpper();  //secure3dhash
            ////////////////////////////////////////////
            var values = new Dictionary<string, string>
{
   { "cardnumber", KartNo },
   { "cardcvv2", Cvv },
   { "cardexpiredatemonth", KartAy},
   { "cardexpiredateyear", KartYil },
   { "mode", Mode },
   { "apiversion", ApiVersion },
   { "terminalprovuserid", TerminalProvUserId },
   { "terminaluserid", TerminalUserId },
   { "terminalmerchantid", TerminalMerchantId },
   { "txntype", TxnType },
   { "txnamount", TxnAmount },
   { "txncurrencycode",CurrencyCode},
   { "txninstallmentcount",InstallmentCount },
   { "orderid",OrderId },
   { "terminalid", TerminalId },
   { "successurl", SuccessUrl },
   { "errorurl", ErrorUrl},
   { "customeripaddress", CustomerIpAdress },
   { "secure3dhash", Secure3dHash },
   { "secure3dsecuritylevel",Secure3dSecurityLevel }
            };




            //////////////////////////////////////////////////////
            StringBuilder sb = new StringBuilder();
            sb.Append("<form style=\"display: none;\" id=\"garantiform\" method=\"POST\" action=\"" + posturl + "\">");
            foreach (var item in values)
            {
                sb.AppendLine("<input type=\"hidden\" name=\"" + item.Key + "\" value=\"" + item.Value + "\" >");
            }

            sb.AppendLine("</form>");
            sb.Append("<script>document.getElementById(\"garantiform\").submit();</script>");
            ////////////////////////////////////////////////////////
            Response.Write(sb.ToString());
            //var content = new FormUrlEncodedContent(values);

            //var response = client.PostAsync("https://sanalposprovtest.garanti.com.tr/servlet/gt3dengine", content);

            //var responseString = response.Result.Content.ReadAsStringAsync();
        }
    }
}