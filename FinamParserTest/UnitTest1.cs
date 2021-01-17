using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FinamParser;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace FinamParserTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public  void TestMethod1()
        {
            var uri = @"http://export.finam.ru/export9.out?market=24&em=18947&token=03AGdBq24ahTFv9-VifwVus3BJoBRIa_oqcTo3J0UsnGfRSxcBo7XcV2UB0TgvK1FP9J-1AoJNhlJmV-pY0g9wQ65TAaD2msMK1yskU6Q9xXEiBD_9c02qBP6f4UPmjuqoS066cIPbo-0HwjwQe7VNsJ3ElUpRULPgdmECfrrhL3jYZKnLZjEGz_Kn29paN3YbEUgwUlVtN2WTcjGjAfg8BVNuDHq_XV1p3xeG6P2enC9L0_bdcU-OyAqat8V9fSINOkmeWhJzdegxTQqewHr_KL-PXMH-Nz54GxZqJY8NfP0zva1qP-RIlWZqzfWzPVGE1mdrtsljaBMYH80Gumfpljd2yieB0EWRaCtMRY4uzHrh6rOLN03Jx3gzznP_3k42tcXS7lfJSqTmad96Ipjn93muyuEuii3jT4EqXfPHdf__hNCN4486sHOeTDgnE_dqsGJWemvnVrYd&code=NYMEX.PL&apply=0&df=1&mf=11&yf=2020&from=01.12.2020&dt=2&mt=11&yt=2020&to=02.12.2020&p=7&f=NYMEX.PL_201201_201202&e=.txt&cn=NYMEX.PL&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=1&sep2=1&datf=1&at=1";
            var uri2 = @"http://export.finam.ru/export9.out?market=1&em=8&token=03AGdBq25wG0ZL7eCHrHWdcpR2bmtgBF--21z0m-tTLFbpQSFF5hDKBSgXywNSYf9r1kSBupWrvQ9SiCDKdGbsQOwfQtMICp1_hTErlQiomgygGvZgkmD7q-27jHlKX8QFZxF4umeaDBIKClnnhbUE4WQd2IP7saDK9Oac_Wk6rKB4JeBtfT09x-YZsYYSTVrIwKV8RTvnGX_X6oSR5UGrHidUy9QjtWrBdoMjUHmRSknOx3FWO4goDX84KTRfk5a45uPVMryOxYFLQGr6sqrh9Cgfr6PhQCHHJthI1s7BjM1CLJpbxS1uXNMIpwWrLb_JadmBrNrtKUHb_pWUqYvQlaR_8XBBi7js4D9QhJ56o1efTjbvnFqZvNnz5IoYshFpujzHNdIzhwjVeixshZeEINNczq29Ut8tVd0TQbzP10kawJiV3wUNeKYR9PXapz-yoJmBH9RokRnu&code=LKOH&apply=0&df=31&mf=11&yf=2020&from=31.12.2020&dt=10&mt=0&yt=2021&to=10.01.2021&p=8&f=LKOH_201231_210110&e=.txt&cn=LKOH&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=1&sep2=1&datf=1&at=1";
            var parser = new FinamParser.FinamParser(uri);
            var parser2 = new FinamParser.FinamParser(uri2,",",false);
            Task<FinamResult> t1= parser2.makeRequest(new DateTime(2020, 12, 1), new DateTime(2021, 1, 10));
            Thread.Sleep(3000); //Finam has multi-parsing protection
            Task<FinamResult> t2 = parser.makeRequest(new DateTime(2020, 1, 15), new DateTime(2020, 1, 20));
            Task.WaitAll(t1, t2);

            Assert.IsTrue(t1.Result.rows.Count > 1);
            Assert.AreEqual(((string[])t1.Result.rows[1])[0],  "LKOH");

            Assert.IsTrue(t2.Result.rows.Count > 1);
            var s = (IDictionary<string, string>) t2.Result.rows[0];
            Assert.AreEqual(s["<TICKER>"], "NYMEX.PL");
        }
    }
}
