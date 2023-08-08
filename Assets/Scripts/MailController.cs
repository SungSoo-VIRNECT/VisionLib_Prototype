using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

struct SendMailJobSystem : IJob
{
    public static string MailAddress { set; get; }
    public static string Name { set; get; }
    public static string ToMailAddress { set; get; }
    public static string AttachedFileName { set; get; }

    public static Action OnMailSended;

    public void Execute()
    {
        MailAddress From = new MailAddress(MailAddress, Name);
        MailAddress To = new MailAddress(ToMailAddress);
        MailMessage mail = new MailMessage(From, To);
        mail.Subject = "미륵사 중문 기념 사진";
        mail.Body = "미륵사 중문 기념 사진";
        Attachment data = new Attachment(AttachedFileName, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        mail.Attachments.Add(data);
        // 

        SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            //Credentials = new System.Net.NetworkCredential(MailAddress, "seok7157") as ICredentialsByHost,
#if UNITY_ANDROID
            Credentials = new System.Net.NetworkCredential(MailAddress, "Password") as ICredentialsByHost,
#else
            Credentials = new System.Net.NetworkCredential(MailAddress, "Password") as ICredentialsByHost,
#endif
            EnableSsl = true
        };
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

        client.Send(mail);

        Debug.Log("Send mail!");

        if (OnMailSended != null)
        {
            OnMailSended.Invoke();
        }
    }
}


public class MailController : MonoBehaviour
{
    [SerializeField]
    private GameObject Root;
    public Button CancelButton;
    public Button SendMailButton;
    public Button ReTakeButton;

    private string MailAddress = "jh.jo890618@gmail.com";
    private string Name = "virnect ar_client";

    [SerializeField]
    private string ToMailAddress = "jh.jo@virnect.com";

    private string AttachedFileName = "";

    //public EmailValidator emailValidator;
    public UnityEvent OnNetworkConnection;
    public UnityEvent OnEmailValidation;
    public UnityEvent OnEmailOkay;

    //public CaptureScreenShot CaptureScreenShotInstance;

    private bool IsMailSended = false;

    public void ShowMailControllerUI()
    {
        Root.SetActive(true);
    }

    public void HideMailControllerUI()
    {
        Root.SetActive(false);
    }

    public void SetAttachedFileName(string filename)
    {
        AttachedFileName = filename;
#if UNITY_IOS
        AttachedFileName = Path.Combine(Application.persistentDataPath, filename);
#endif
    }

    public void SetMailAddress(string mailAddress)
    {
        Debug.Log("ToMailAddress :" + mailAddress);
        ToMailAddress = mailAddress;
    }

    public void Send()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("NetworkReachability.NotReachable");
            OnNetworkConnection.Invoke();
            return;
        }
        //else if (!emailValidator.IsValid())
        //{
        //    Debug.Log("EmailValidator.IsValue is false");
        //    OnEmailValidation.Invoke();
        //    return;
        //}
        StartCoroutine(sendMailCheck());
        Debug.Log("ToMailAddress :" + ToMailAddress);
        SendMailJobSystem.MailAddress = MailAddress;
        SendMailJobSystem.Name = Name;
        SendMailJobSystem.ToMailAddress = ToMailAddress;
        SendMailJobSystem.AttachedFileName = AttachedFileName;
        SendMailJobSystem.OnMailSended += () =>
        {
            Debug.Log("OnMailSended event invoked in JobSystem thread.");
            IsMailSended = true;
        };
        SendMailJobSystem sendMailJobSystem = new SendMailJobSystem();

        sendMailJobSystem.Schedule();

        
    }

    IEnumerator sendMailCheck()
    {
        while(IsMailSended == false)
        {
            OnEmailOkay?.Invoke();
            yield return null;
        }
        IsMailSended = false;
    }
}
