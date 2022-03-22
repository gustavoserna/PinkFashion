using System;
namespace PinkFashion
{
    public class Constantes
    {

        //public static string url = "http:/ /pinkfashionstore.mx/app/";
        //public static string root_url = "http:/ /pinkfashionstore.mx/admin";

        //public static string url = "https://pinkfashionstore.com/app/";
        //public static string root_url = "https://pinkfashionstore.com/admin/";
        public static string url = "http://pink.resosistemas.mx/app/";
        public static string root_url = "http://pink.resosistemas.mx/admin/";
        public static string url_img = "http://pink.resosistemas.mx/admin/imgMaestra/{0}/{1}";
        public static string open_pay_url = "https://pink.resosistemas.mx/backend/";
        public static string tienda = "";


        //OAuth
        public static string AppName = "PinkFashion";
        public static string iOSClientId = "669401120691-jngln8cs6vbl3tbsv1k45thoet20l3h8.apps.googleusercontent.com";
        public static string AndroidClientId = "669401120691-sgsi4m7e41p4ris3lobp1l5j371p88nn.apps.googleusercontent.com";

        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        public static string iOSRedirectUrl = "com.googleusercontent.apps.669401120691-jngln8cs6vbl3tbsv1k45thoet20l3h8";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.669401120691-sgsi4m7e41p4ris3lobp1l5j371p88nn";

        //Algolia
        public static string algolia_app_id = "S68HLB20KI";
        public static string algolia_api_key = "7ccb6cb4d74b40efccc3b67904278258";
        //public static string algolia_api_key = "ebbc9f83e35955c507a012c72a42ff41";
        public static string algolia_url = "https://S68HLB20KI.algolia.net/1/indexes/productos_pink/query";
    }
}
