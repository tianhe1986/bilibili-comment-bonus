using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Forms;
using bili_bonus.data;

namespace bili_bonus
{
    public partial class Form1 : Form
    {
        private string dynamicIdTxt;
        private string bonusNumTxt;

        private string outputStr;
        private Dictionary<string, bool> mids;

        private bool isRun;

        private SynchronizationContext context;

        public Form1()
        {
            InitializeComponent();

            this.context = SynchronizationContext.Current;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.outputTxt.Text = "You are shock!" + Environment.NewLine;

            this.dynamicIdTxt = this.dynamicId.Text;

            Console.WriteLine("输入的动态id是" + this.dynamicIdTxt);

            this.bonusNumTxt = this.bonusNum.Text;

            Console.WriteLine("输入的抽奖人数是" + this.bonusNumTxt);

            ThreadStart threadStart = new ThreadStart(doRandBonus);
            Thread childThread = new Thread(threadStart);
            childThread.Start();
        }

        private void updateTxtFunc(object text)
        {
            this.outputTxt.Text = text.ToString();
            this.outputTxt.SelectionStart = this.outputTxt.Text.Length;
            this.outputTxt.ScrollToCaret();
        }

        private void resetForRun()
        {
            this.outputStr = "";
            this.mids = new Dictionary<string, bool>();

            this.context.Post(updateTxtFunc, this.outputStr);
        }

        private void doRandBonus()
        {
            int bonusNum;
            if (!int.TryParse(this.bonusNumTxt, out bonusNum))
            {
                this.outputTxt.Text = "抽奖人数有误，请重新输入" + Environment.NewLine;

                return;
            }

            if (this.isRun)
            {
                return;
            }

            string dynamicId = this.dynamicIdTxt;

            this.isRun = true;

            this.resetForRun();

            // 获取动态详情，以判断接下来获取评论的参数
            string oid;
            int dynamicType;

            this.outputStr += "Bilibili评论抽奖工具v2.0 by 天河" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            this.context.Post(updateTxtFunc, this.outputStr);

            this.getDynamicParam(dynamicId, out oid, out dynamicType);
            Thread.Sleep(500);

            this.outputStr += "获取动态详情完成" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            this.context.Post(updateTxtFunc, this.outputStr);

            int paramType = 17;
            if (dynamicType == 2)
            {
                paramType = 11;
            }

            string initUrl = "https://api.bilibili.com/x/v2/reply?jsonp=jsonp&type=" + paramType.ToString() +"&sort=0&oid=" + oid;

            ArrayList list = this.getPagesCycle(initUrl);
            for (int i = 0; i < 8; i++)
            {
                this.outputStr += Environment.NewLine;
            }
            this.context.Post(updateTxtFunc, this.outputStr);

            if (list.Count <= bonusNum)
            {
                this.outputStr += "评论人数小于等于抽奖人数，不用抽了吧" + Environment.NewLine;
            }
            else
            {
                this.outputStr += "获取评论按时间排序，这" + bonusNum.ToString() + "位幸运的中奖者为：" + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                Random random = new Random();
                for (int i = 0; i < bonusNum; i++)
                {
                    int bonusIndex = random.Next(list.Count);
                    ArrayList item = (ArrayList)list[bonusIndex];

                    string tempStr = (string)item[1] + "   https://space.bilibili.com/" + (string)item[0] + "                      对应评论为第" + ((int)item[2]).ToString() + "页第" + ((int)item[3]).ToString() + "条" + Environment.NewLine;
                    this.outputStr += tempStr;

                    list.RemoveAt(bonusIndex);
                }
            }

            this.context.Post(updateTxtFunc, this.outputStr);
            this.isRun = false;
        }

        private void getDynamicParam(string dynamicId, out string oid, out int dynamicType)
        {
            string url = "https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail?dynamic_id=" + dynamicId;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);

            string responseTxt = myreader.ReadToEnd();
            myreader.Close();

            DynamicResResult dynamicResResult = new DynamicResResult();
            dynamicResResult = JsonConvert.DeserializeObject<DynamicResResult>(responseTxt);

            dynamicType = dynamicResResult.data.card.desc.type;

            if (dynamicType == 2)
            {
                oid = dynamicResResult.data.card.desc.rid_str;
            }
            else
            {
                oid = dynamicId;
            }
        }

        private ArrayList getPagesCycle(string url)
        {
            ArrayList list = new ArrayList();

            ArrayList tempList = new ArrayList();

            bool res = this.getOnePage(url, 1, out tempList);
            list.AddRange(tempList);

            int offsetIndex = 2;

            while (res)
            {
                Thread.Sleep(1000);
                string offsetUrl = url + "&pn=" + offsetIndex.ToString();
                tempList = new ArrayList();
                res = this.getOnePage(offsetUrl, offsetIndex, out tempList);
                list.AddRange(tempList);

                offsetIndex++;
            }

            return list;
        }

        private bool getOnePage(string url, int pageIndex, out ArrayList resList)
        {
            resList = new ArrayList();
            bool res = true;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);

            string responseTxt = myreader.ReadToEnd();
            myreader.Close();

            // 解析Json
            ResResult resResult = new ResResult();
            resResult = JsonConvert.DeserializeObject<ResResult>(responseTxt);

            if (resResult.data.replies != null)
            {
                int offset = 1;
                foreach (ResReply item in resResult.data.replies)
                {
                    ResReplyMember member = item.member;

                    if (this.mids.ContainsKey(member.mid))
                    {
                        offset++;
                        continue;
                    }

                    this.mids.Add(member.mid, true);
                    ArrayList tempList = new ArrayList();
                    tempList.Add(member.mid);
                    tempList.Add(member.uname);
                    tempList.Add(pageIndex);
                    tempList.Add(offset);
                    offset++;

                    resList.Add(tempList);
                }
                this.outputStr += "获取到第" + pageIndex.ToString() + "页评论" + Environment.NewLine;
                this.context.Post(updateTxtFunc, this.outputStr);
            }
            else
            {
                res = false;
            }

            return res;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
