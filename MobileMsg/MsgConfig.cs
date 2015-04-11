using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMPPLib;
using CSMP.BLL;
using CSMP.Model;
using Tool;

namespace MobileMsg
{
    public class MsgConfig
    {
        public EMPPLib.emptcl Empp ;
        public MsgConfig()
        {
            Empp = new emptcl();
            Empp.SubmitRespInterface += (new _IemptclEvents_SubmitRespInterfaceEventHandler(SubmitRespInterface));

            

        }

        public void Send(string Msg, List<string> MobileNumbers)
        {
            Msg = Msg.Trim();
            if (Msg.Length>60)
            {
                Msg = Msg.Substring(0, 59);
            }
            if (MobileNumbers==null||MobileNumbers.Count==0)
            {
                return;
            }
            string Host=ProfileBLL.GetValue(ProfileInfo.UserKey.MobileMsgKey.Host, true);
            string Port=ProfileBLL.GetValue(ProfileInfo.UserKey.MobileMsgKey.Port, true);
            string AccountID=ProfileBLL.GetValue(ProfileInfo.UserKey.MobileMsgKey.AccountID, true);
            string PassWord=ProfileBLL.GetValue(ProfileInfo.UserKey.MobileMsgKey.PassWord, true);
            string ServiceID=ProfileBLL.GetValue(ProfileInfo.UserKey.MobileMsgKey.ServiceID, true);


            EMPPLib.Mobiles MobInfo = new EMPPLib.MobilesClass();
            foreach (string item in MobileNumbers)
            {
                MobileNumbers.Add(item.Trim());
            }
            EMPPLib.ShortMessage ShortMsgInfo = new ShortMessageClass();
            ShortMsgInfo.srcID =AccountID;
            ShortMsgInfo.ServiceID = ServiceID;
            ShortMsgInfo.needStatus = true;
            ShortMsgInfo.content = Msg;
            ShortMsgInfo.DestMobiles = MobInfo;
            ShortMsgInfo.SendNow = true;
            Empp.needStatus = true;


            try
            {
                if (Empp.connect(Host,Port,)
                {
                    
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
