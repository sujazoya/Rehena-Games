using SimpleJSON;
using UnityEngine;
using suja;

public class GoogleSheetHandler
{


    // public static string firebaseDatabaseLink = "https://plumber-f56da-default-rtdb.firebaseio.com/";

    public static bool show_ad_as_index;
    public static string ad_app_name;
    public static bool ad_cancel_buton;
    public static bool net_warn_cancel_buton;
    public static string ad_app_short_desc;
    public static string ad_app_url;
    public static string ad_banner_url;
    public static string ad_dialog_title;
    public static string ad_icon_url;
    public static string ad_message;
    public static bool isAds;
    public static bool isNotification;
    public static bool isUpdate;
    public static string not_dialog_title;
    public static string not_message;
    public static bool not_show_dialog;
    public static string update_app_url;
    public static string update_dialog_title;
    public static string update_message;
    public static bool update_show_cancel;
    public static string update_title;
    public static string update_version_name;
    public static string extraStringPara1;
    public static string extraStringPara2;
    public static string g_app_id;
    public static string g_banner1;
    public static string g_banner2;
    public static string g_inter1;
    public static string g_inter2;
    public static string g_inter3;
    public static string g_rewarded1;
    public static string g_rewarded2;
    public static string g_rewarded3;
    public static string g_rewardedint1;
    public static string g_rewardedint2;
    public static string g_rewardedint3;
    public static string sa_ad_count;
    public static bool showAds;   
    public static bool show_g_banner1;
    public static bool show_g_banner2;
    public static bool show_g_inter1;
    public static bool show_g_inter2;
    public static bool show_g_inter3;
    public static bool show_g_rewardedint1;
    public static bool show_g_rewardedint2;
    public static bool show_g_rewardedint3;
    public static bool show_g_rewarded1;
    public static bool show_g_rewarded2;
    public static bool show_g_rewarded3;
    public static bool show_loading;
    public static bool show_startapp;
    public static string ad1_image;
    public static string ad1_url;
    public static string ad2_image;
    public static string ad2_url;
    public static string button_text;
    public static string other_games;

    public static string terms_url;
    public static bool has_terms;

    public static bool googlesheetInitilized=false;

    private static int deviceType = 0;

    public static string localInfo;
    private static bool BooleanChecker(string value)
    {
        string tmpstring = value.ToLower();
        if (tmpstring == "true") return true;
        else return false;
    }
    public static void GetDataFromAPI(string snapshot)
    {
        JSONNode info = JSON.Parse(snapshot)[0];
        show_ad_as_index = BooleanChecker(info["show_ad_as_index"].Value);
        isAds = BooleanChecker(info["isAds"].Value);
        isNotification = BooleanChecker(info["isNotification"].Value);
        isUpdate = BooleanChecker(info["isUpdate"].Value);
        not_dialog_title = info["not_dialog_title"].Value.ToString();
        not_message = info["not_message"].Value.ToString();
        button_text = info["button_text"].Value.ToString();
        not_show_dialog = BooleanChecker(info["not_show_dialog"].Value);
        update_app_url = info["update_app_url"].Value.ToString();
        update_dialog_title = info["update_dialog_title"].Value.ToString();
        update_message = info["update_message"].Value.ToString();
        update_show_cancel = BooleanChecker(info["update_show_cancel"].Value);
        update_title = info["update_title"].Value.ToString();
        update_version_name = info["update_version_name"].Value.ToString();
        showAds = BooleanChecker(info["showAds"].Value);
        other_games= info["other_games"].Value.ToString();

        ad_cancel_buton = BooleanChecker(info["ad_cancel_buton"].Value);
        net_warn_cancel_buton = BooleanChecker(info["net_warn_cancel_buton"].Value);

        show_g_banner1 = BooleanChecker(info["show_g_banner1"].Value);
        show_g_banner2 = BooleanChecker(info["show_g_banner2"].Value);
        show_g_inter1 = BooleanChecker(info["show_g_inter1"].Value);
        show_g_inter2 = BooleanChecker(info["show_g_inter2"].Value);
        show_g_inter3 = BooleanChecker(info["show_g_inter3"].Value);
        show_g_rewarded1 = BooleanChecker(info["show_g_rewarded1"].Value);
        show_g_rewarded2 = BooleanChecker(info["show_g_rewarded2"].Value);
        show_g_rewarded3 = BooleanChecker(info["show_g_rewarded3"].Value);

        show_g_rewardedint1 = BooleanChecker(info["show_g_rewardedint1"].Value);
        show_g_rewardedint2 = BooleanChecker(info["show_g_rewardedint2"].Value);
        show_g_rewardedint3 = BooleanChecker(info["show_g_rewardedint3"].Value);

        show_loading = BooleanChecker(info["show_loading"].Value);
        show_startapp = BooleanChecker(info["show_startapp"].Value);

        ad_app_name = info["ad_app_name"].Value.ToString();
        ad_app_short_desc = info["ad_app_short_desc"].Value.ToString();
        ad_banner_url = info["ad_banner_url"].Value.ToString();
        ad_dialog_title = info["ad_dialog_title"].Value.ToString();
        ad_icon_url = info["ad_icon_url"].Value.ToString();
        ad_message = info["ad_message"].Value.ToString();

        terms_url = info["terms_url"].Value.ToString();
        has_terms = BooleanChecker(info["has_terms"].Value);

        extraStringPara1 = info["extraStringPara1"].Value.ToString();
        extraStringPara2 = info["extraStringPara2"].Value.ToString();
        g_app_id = info["g_app_id"].Value.ToString();
        ad_app_url = info["ad_app_url"].Value.ToString();     

        g_banner1 = info["g_banner1"].Value.ToString();
        g_banner2 = info["g_banner2"].Value.ToString();
       
        g_inter1 = info["g_inter1"].Value.ToString();
        g_inter2 = info["g_inter2"].Value.ToString();
        g_inter3 = info["g_inter3"].Value.ToString();
        g_rewarded1 = info["g_rewarded1"].Value.ToString();
        g_rewarded2 = info["g_rewarded2"].Value.ToString();
        g_rewarded3 = info["g_rewarded3"].Value.ToString();

        g_rewardedint1 = info["g_rewardedint1"].Value.ToString();
        g_rewardedint2 = info["g_rewardedint2"].Value.ToString();
        g_rewardedint3 = info["g_rewardedint3"].Value.ToString();

        sa_ad_count = info["sa_ad_count"].Value.ToString();
        ad1_image = info["ad1_image"].Value.ToString();
        ad1_url = info["ad1_url"].Value.ToString();
        ad2_image = info["ad2_image"].Value.ToString();

        googlesheetInitilized = true;       
        GameHandler.Instance.CheckForNtification();
        GameHandler.isGoogleSheetDataConfirmed = true;
        GameHandler.Instance.CheckForUpdate();
        GameHandler.Instance.ShowTerms();
        AdmobAdmanager.Instance.GestAdIDsAndRequestAds();



        //localInfo = not_message.ToString();

        //GetData(g_banner1); g_banner1 = RetrieveFromDatabase(g_banner1);
        //g_banner1 = RetrieveFromDatabase(g_banner1);


    }
    //public static string SaveToString(string text)
    //{
    //    return JsonUtility.ToJson(text);
    //}
    //static FirebaseData_ firebaseData_ = new FirebaseData_();
    //public static string databaseName;
    //public static string databaseValue;
    //static void GetData(string data)
    //{
    //    databaseName = data;
    //    databaseValue = data;
    //    RetrieveFromDatabase(data);        
    //}
    //private static string RetrieveFromDatabase(string data)
    //{
    //    FirebaseData_ actfire;
    //    RestClient.Get<FirebaseData_>(firebaseDatabaseLink+ data + ".json").Then(response =>
    //    {
    //        firebaseData_ = response;
    //        return firebaseData_.value;
    //    });
    //    return firebaseData_.value;
    //}

}
