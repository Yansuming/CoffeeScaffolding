using System.Net.Mail;
using System.Text;

namespace CoffeeScaffolding.CoffeeScaffoldingUtil.Msg
{
    /// <summary>
    /// 电子邮件发送工具类
    /// </summary>
    public class Email
    {
        private readonly string EmailServerHost;
        private readonly string EmailAddress;
        private readonly SmtpClient _smtpClient;

        public Email(string EmailServerHost,string EmailAddress,string EmailPassword)
        {
            this.EmailServerHost = EmailServerHost;
            this.EmailAddress = EmailAddress;
            _smtpClient = new SmtpClient();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="anonymous">是否匿名发送</param>
        /// <param name="strFrom">发件箱</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="strPwd">密码</param>
        /// <param name="strTo">收件人(用;分割)</param>
        /// <param name="title">标题</param>
        /// <param name="content">邮件正文</param>
        /// <param name="mailAtt">附件路径</param>
        /// <param name="strCc">抄送人(用;分割)</param>
        /// <returns></returns>
        /// <author>Eric</author>
        /// <update>Yan,niu</update>
        /// <date>2015-05-14</date>
        public static string SendMail(string host, bool anonymous, string strFrom, string displayName, string strPwd, string strTo, string title, string content, string mailAtt, string strCc)
        {
            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = host;//"CORIMC04";//指定SMTP服务器
            if (!anonymous)
            {
                _smtpClient.Credentials = new System.Net.NetworkCredential(strFrom, strPwd);//用户名和密码; 匿名发送不要
            }

            MailMessage _mailMessage = new MailMessage();
            _mailMessage.From = new MailAddress(strFrom, displayName, Encoding.GetEncoding("UTF-8"));
            string[] strTemp = strTo.Split(';');
            foreach (string to in strTemp)
            {
                if (to.ToString() != "")
                {
                    _mailMessage.To.Add(new MailAddress(to, "", Encoding.GetEncoding("UTF-8")));
                }
            }
            strTemp = strCc.Split(';');
            foreach (string Cc in strTemp)
            {
                if (Cc.ToString().Trim() != "")
                {
                    _mailMessage.CC.Add(new MailAddress(Cc, "", Encoding.GetEncoding("UTF-8")));
                }
            }

            _mailMessage.Subject = title;//主题
            _mailMessage.Body = content;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式

            //添加附件
            if (mailAtt != null)
            {
                foreach (string att in mailAtt.Split(';'))
                {
                    if (att.Length > 0 && File.Exists(att))
                        _mailMessage.Attachments.Add(new Attachment(att));
                }
            }

            try
            {
                _smtpClient.Send(_mailMessage); // 发送邮件

                //释放附件
                _mailMessage.Attachments.Dispose();
                return title + ",发送成功!";
            }
            catch (SmtpException ex)
            {
                //释放附件
                _mailMessage.Attachments.Dispose();
                return title + ",发送失败!" + ex.Message;
            }

        }
    }
}
