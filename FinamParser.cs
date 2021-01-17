using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;

namespace FinamParser
{
    public class FinamResult
    {
        public string status { get; set; }
        public ArrayList rows;     
        public FinamResult()
        {
            rows = new ArrayList();
        }
        public void Add(string[] row)
        {
            rows.Add(row);
        }
        public void Add(string[] key,string[] value)
        {
            var d = new Dictionary<string, string>();  
            for(int i=0; i < key.Length; i++)
            {
                d.Add(key[i], value[i]);
            }
            rows.Add(d);
        }
    }
    public class FinamParser
    {        
        private string uri { get;  }
        private string delimeter;
        private bool withHeader;

        private Uri replaceDates(string uri, DateTime dtFrom, DateTime dtTo)
        {
            //&yf=2020&from=01.12.2020&dt=2&mt=11&yt=2020&to=02.12.2020&            
            uri = Regex.Replace(uri, @"(&yf=)\d{4}&", "&yf=" + dtFrom.Year.ToString()+"&");
            uri = Regex.Replace(uri, @"(&yt=)\d{4}&", "&yt="+dtTo.Year.ToString()+"&");
            uri = Regex.Replace(uri, @"(&from=)\d{1,2}\.\d{1,2}\.\d{4}&", "&from=" + dtFrom.ToString("dd.MM.yyyy")+"&");
            uri = Regex.Replace(uri, @"(&to=)\d{1,2}\.\d{1,2}\.\d{4}&", "&to=" + dtTo.ToString("dd.MM.yyyy")+"&");
            return new Uri(uri);
        }
        public FinamParser(string uri,string delimeter=",",bool withHeader=true) 
        {
            this.uri = uri;
            this.delimeter = delimeter;
            this.withHeader = withHeader;
        }

        public async Task<FinamResult> makeRequest(DateTime dtFrom, DateTime dtTo)
        {
            var uri = replaceDates(this.uri, dtFrom, dtTo);

            HttpClient client = new HttpClient();
            FinamResult result=new FinamResult();

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                result.status = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string[] rows = responseBody.Split('\n');
                    string[] hdrTitle=null;
                    if (withHeader)
                    {
                        hdrTitle = rows[0].Split(this.delimeter.ToCharArray());
                        rows=rows.Skip(1).ToArray();
                    }
                    foreach (string s in rows)
                    {
                        if (withHeader)
                            result.Add(hdrTitle,s.Split(this.delimeter.ToCharArray()));
                        else
                            result.Add(s.Split(this.delimeter.ToCharArray()));
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                result.status = e.Message;
                return result;
            }

        }
    }
}
