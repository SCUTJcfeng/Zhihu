using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace ZhihuUser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Login(base_url);
            //Jsonjiexi(content);
            Task.Run(() =>
            {
                while (true)
                {
                    /*
                    string sql = @"SELECT *
                FROM `zhihu` AS t1 JOIN (SELECT ROUND(RAND() * ((SELECT MAX(follower_count) FROM `zhihu`)-(SELECT MIN(follower_count) FROM `zhihu`))+(SELECT MIN(follower_count) FROM `zhihu`)) AS follower_count) AS t2
                WHERE t1.follower_count >= t2.follower_count
                ORDER BY t1.follower_count LIMIT 1;";
                */
                    string sql = @"SELECT url_token FROM zhihu ORDER BY RAND() LIMIT 1";
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();
                    command = new MySqlCommand(sql, connection);
                    //System.Data.Common.DbDataReader dbDataReader = await command.ExecuteReaderAsync();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        next_url_token = reader["url_token"].ToString();
                    reader.Close();
                    reader.Dispose();
                    is_end = "False";
                    page = -20;
                    //next_url_token = "kaifulee";
                    while (is_end == "False")
                    {
                        page += 20;
                        next_url = base_url_part_1 + next_url_token + base_url_part_2 + page;
                        Login(next_url);
                        Jsonjiexi(content);
                    }
                }
            });
            textBox1.AppendText("Starts..");
        }
        #region 定义field
        private CookieContainer cookies = new CookieContainer();
        private MySqlConnection connection;
        private MySqlCommand command;
        private string reportFile;
        private string content;
        private string next_url;
        private const string base_url_part_1 = "Https://www.zhihu.com/api/v4/members/";
        private const string base_url_part_2 = "/followers?include=data[*].answer_count,articles_count,gender,follower_count,badge[?(type=best_answerer)].topics&limit=20&offset=";//followees为该url_token关注的名单，followers为粉丝列表
        private const string base_url_token = "excited-vczh";
        private string next_url_token;

        private const string base_url = "https://www.zhihu.com/api/v4/members/excited-vczh/followees?include=data[*].answer_count,articles_count,gender,follower_count,badge[?(type=best_answerer)].topics&limit=20&offset=0";

        private string[] url_token_list;
        private string[] url_list; //base_url_part_1 + url_token_list[0] + base_url_part_2 + page;
        private int page = 0;

        private string is_end;
        private string totals;
        private string is_start;
        private string previous;
        private string next;

        private JObject paging;
        private JArray data;
        private JArray badge;
        private JArray topics;
        //private JArray topics_2;
        //private JArray topics_3;


        //private string avatar_url_template;
        private string user_type;
        private string answer_count;
        private string type;
        private string url_token;
        private string id;
        private string articles_count;
        private string name;
        private string is_advertiser;
        private string url;
        private string gender;
        private string headline;
        //private string avatar_url;
        private string is_org;
        private string follower_count;
        private string badge_type;
        private string badge_description;

        private string topics_1_url;
        //private string topics_avatar_url;
        private string topics_1_name;
        private string topics_1_introduction;
        private string topics_1_type;
        private string topics_1_excerpt;
        private string topics_1_id;

        //private string topics_2_url;
        ////private string topics_avatar_url;
        //private string topics_2_name;
        //private string topics_2_introduction;
        //private string topics_2_type;
        //private string topics_2_excerpt;
        //private string topics_2_id;

        //private string topics_3_url;
        ////private string topics_avatar_url;
        //private string topics_3_name;
        //private string topics_3_introduction;
        //private string topics_3_type;
        //private string topics_3_excerpt;
        //private string topics_3_id;

        #endregion

        private void Jsonjiexi(string content)
        {
            is_end = "";
            totals = "";
            is_start = "";
            previous = "";
            next = "";
            JObject root = JsonConvert.DeserializeObject<JObject>(content);
            paging = (JObject)root["paging"];
            data = (JArray)root["data"];
            #region paging
            is_end = paging["is_end"].ToString();
            totals = paging["totals"].ToString();
            is_start = paging["is_start"].ToString();
            previous = paging["previous"].ToString();
            next = paging["next"].ToString();
            #endregion
            for (int i = 0; i < data.Count; i++)
            {
                Initialize();
                badge = (JArray)data[i]["badge"];
                //avatar_url_template = data[i]["avatar_url_template"].ToString();
                user_type = data[i]["user_type"].ToString();
                answer_count = data[i]["answer_count"].ToString();
                type = data[i]["type"].ToString();
                url_token = data[i]["url_token"].ToString();
                id = data[i]["id"].ToString();
                articles_count = data[i]["articles_count"].ToString();
                name = data[i]["name"].ToString();
                is_advertiser = data[i]["is_advertiser"].ToString();
                url = data[i]["url"].ToString();
                gender = data[i]["gender"].ToString();
                headline = data[i]["headline"].ToString();
                //avatar_url = data[i]["avatar_url"].ToString();
                is_org = data[i]["is_org"].ToString();
                follower_count = data[i]["follower_count"].ToString();
                if (badge.Count != 0)
                {
                    try
                    {
                        topics = (JArray)badge[0]["topics"];
                        topics_1_url = topics[0]["url"].ToString();
                        //topics_avatar_url = topics[0]["avatar_url"].ToString();
                        topics_1_name = topics[0]["name"].ToString();
                        topics_1_introduction = topics[0]["introduction"].ToString();
                        topics_1_type = topics[0]["type"].ToString();
                        topics_1_excerpt = topics[0]["excerpt"].ToString();
                        topics_1_id = topics[0]["id"].ToString();

                        ////topics_2 = (JArray)badge[1]["topics"];
                        //topics_2_url = topics[1]["url"].ToString();
                        ////topics_avatar_url = topics[1]["avatar_url"].ToString();
                        //topics_2_name = topics[1]["name"].ToString();
                        //topics_2_introduction = topics[1]["introduction"].ToString();
                        //topics_2_type = topics[1]["type"].ToString();
                        //topics_2_excerpt = topics[1]["excerpt"].ToString();
                        //topics_2_id = topics[1]["id"].ToString();

                        ////topics_3 = (JArray)badge[2]["topics"];
                        //topics_3_url = topics[2]["url"].ToString();
                        ////topics_avatar_url = topics[2]["avatar_url"].ToString();
                        //topics_3_name = topics[2]["name"].ToString();
                        //topics_3_introduction = topics[2]["introduction"].ToString();
                        //topics_3_type = topics[2]["type"].ToString();
                        //topics_3_excerpt = topics[2]["excerpt"].ToString();
                        //topics_3_id = topics[2]["id"].ToString();
                    }
                    catch (Exception)
                    {
                    }
                    badge_type = badge[0]["type"].ToString();
                    badge_description = badge[0]["description"].ToString();
                }
                SaveIntoSQL();
            }
            //connection.Close();
        }

        public void Login(string url)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            HttpWebRequest req = Request(url);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception e)
            {
                SoundPlayer player = new SoundPlayer("music_background.wav");
                player.PlayLooping();
                MessageBox.Show(e.Message);
            }

            var len = response.ContentLength;
            Stream resStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);
            content = myStreamReader.ReadToEnd();
            content = HttpUtility.UrlDecode(content, System.Text.Encoding.UTF8);
            resStream.Close();
            response.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reportFile = "report.txt";
            if (!File.Exists(reportFile))
                File.Create(reportFile);

            is_end = "False";
            connection = new MySqlConnection("Host=localhost;user=root;password=root;database=userinfo;charset=utf8");
            connection.Open();
            timer1.Interval = 60000;
            timer1.Enabled = true;
            label1.Text = string.Empty;
            timer2.Interval = 100;
            timer2.Enabled = true;
        }

        private void Initialize()
        {


            //avatar_url_template = "";
            user_type = "";
            answer_count = "";
            type = "";
            url_token = "";
            id = "";
            articles_count = "";
            name = "";
            is_advertiser = "";
            url = "";
            gender = "";
            headline = "";
            //avatar_url = "";
            is_org = "";
            follower_count = "";
            badge_type = "";
            badge_description = "";

            topics_1_url = "";
            topics_1_name = "";
            topics_1_introduction = "";
            topics_1_type = "";
            topics_1_excerpt = "";
            topics_1_id = "";

            //topics_2_url = "";
            //topics_2_name = "";
            //topics_2_introduction = "";
            //topics_2_type = "";
            //topics_2_excerpt = "";
            //topics_2_id = "";

            //topics_3_url = "";
            //topics_3_name = "";
            //topics_3_introduction = "";
            //topics_3_type = "";
            //topics_3_excerpt = "";
            //topics_3_id = "";

        }

        private void SaveIntoSQL()
        {
            try
            {
                name = ReplaceStr(name);
                headline = ReplaceStr(headline);
                string sql = string.Format("insert into userinfo.zhihu(id,name,url_token,follower_count,answer_count,articles_count,user_type,type,is_advertiser,url,gender,headline,is_org,badge_type,badge_description,topics_1_url,topics_1_name,topics_1_introduction,topics_1_type,topics_1_excerpt,topics_1_id) values ('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')", id, name, url_token, follower_count, answer_count, articles_count, user_type, type, is_advertiser, url, gender, headline, is_org, badge_type, badge_description, topics_1_url, topics_1_name, topics_1_introduction, topics_1_type, topics_1_excerpt, topics_1_id);
                command = new MySqlCommand(sql, connection);
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    string txt = "Success Update Database with name = " + name;
                    //textBox1.AppendText(txt + "\r\n");
                    WriteIntoReportText(txt);
                }
            }
            catch (Exception e)
            {
                //if (e.Message.Contains("Duplicate entry"))
                //    return;
                //textBox1.AppendText(e.Message + "\r\n");
                //WriteIntoReportText(name);
            }
        }

        private HttpWebRequest Request(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = true;
            req.Method = "GET";
            req.Accept = "application/json";
            //req.Connection = "keep-alive";
            req.Headers.Add("Origin: https://www.zhihu.com");
            //req.Referer = "https://www.zhihu.com/people/xiao-chou-84/following";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36";
            //req.Headers.Add("Accept-Encoding: deflate, br");
            req.Headers.Add("Accept-Language: zh-CN,zh;q=0.9,zh-TW;q=0.8");
            req.Headers.Add("authorization: Bearer 2|1:0|10:1517207103|4:z_c0|92:Mi4xLVc0WEFBQUFBQUFBQUswR1ZUQVFEU1lBQUFCZ0FsVk5Qd3hjV3dDZ1VhTWsyZks1bXpsTGozSENUWWhwNmpzODhB|a5fca88d556b0f6d63be455fc96b7a01d65604bd7ad0961c64ad27acbd9c4085");
            req.Headers.Add("X-UDID: AACtBlUwEA2PTlaqgHFiA5BtN7bFa1WsOhY=");
            req.Headers.Add("Cookie", @"_zap=63c8a8d0-3b25-462e-b568-491eeb4cad13; q_c1=994a66f9b0a342d294dd7247262a0eaf|1516897210000|1514266887000; l_cap_id=Y2FkY2RlM2NjMjU2NGI0Yzk4ZGUyNDhmM2ZjMzM0ZjI =| 1517156197 | a87cac8bdd99683c6b4e7a524cd22b3f6d8a8c98; r_cap_id=ZTFmOGFjNWI1NDFlNDhmODk4ZTAyZTJiMmQ4NGZiMDA =| 1517156197 | 71731f277527893abf00668476edf2eb4cd5bcf4; cap_id=OTEzODdmODExZDMwNDFmMzk0ZTc3OGIyYWFiNmZkZDM =| 1517156197 | 0f7328cfbb04b6cb7af055456f574a7554ba4eed; __utmz=155987696.1517159437.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __utma=155987696.376294990.1517159437.1517159437.1517161463.2; aliyungf_tc=AQAAAGg7xhFt2gAA+83xPVIMOW9ZmzNv; d_c0=AACtBlUwEA2PTlaqgHFiA5BtN7bFa1WsOhY =| 1517204711; _xsrf=fbacfd6e-9684-4e22-a8dc-c8567cd8c82f; z_c0=2 | 1:0 | 10:1517207103 | 4:z_c0 | 92:Mi4xLVc0WEFBQUFBQUFBQUswR1ZUQVFEU1lBQUFCZ0FsVk5Qd3hjV3dDZ1VhTWsyZks1bXpsTGozSENUWWhwNmpzODhB | a5fca88d556b0f6d63be455fc96b7a01d65604bd7ad0961c64ad27acbd9c4085; capsion_ticket=2 | 1:0 | 10:1517208218 | 14:capsion_ticket | 44:NGM3ZTFhNjhjNjliNGZhNThkNDllMTIyYTdiYjQ4MGY =| ee4631dae21b85df8f5ef563c108571c3e5a550541c35db140c45784aeea34bf");
            return req;
        }

        private void WriteIntoReportText(string txt)
        {
            try
            {
                FileStream file = new FileStream(reportFile, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(file);
                writer.WriteLine(DateTime.Now.ToString() + " : " + txt);
                writer.Flush();
                writer.Close();
                file.Close();
            }
            catch (Exception)
            {

            }
        }

        private string ReplaceStr(string str)
        {
            if (str == "")
                return url;
            else
            {
                str = str.Replace("\\", "\\\\");
                str = str.Replace("'", "\\'");
                return str;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //textBox1.Clear();
            string sql = "select count(*) from zhihu";
            MySqlCommand command = new MySqlCommand(sql, connection);
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            try
            {
                textBox1.AppendText(page.ToString()+' ');
                MySqlDataReader reader = command.ExecuteReader();
                string result = string.Empty;
                while (reader.Read())
                {
                    result = reader[0].ToString();
                }
                reader.Close();
                reader.Dispose();
                //connection.Close();
                button1.Text = result;
                //textBox1.Clear();
            }
            catch (Exception)
            {

                
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            label1.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
