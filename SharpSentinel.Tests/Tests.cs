using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSentinel.Parser;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Data.S1.Annotations;
using SharpSentinel.Parser.Helpers;
using SharpSentinel.Parser.Resources;

namespace SharpSentinel.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task LoadSAFE()
        {
            var paths = new List<string>
            {
                @"C:\Users\Yannik\Desktop\SharpSentinel\S1B_IW_GRDH_1SDV_20180606T050938_20180606T051003_011248_014A48_7E3C.SAFE",
                @"C:\Users\Yannik\Desktop\SharpSentinel\doof\S1B_IW_GRDH_1SDV_20180511T052604_20180511T052629_010869_013E1D_F9FD.SAFE",
                @"C:\Users\Yannik\Desktop\SharpSentinel\S1B_IW_SLC__1SDV_20180606T050847_20180606T050914_011248_014A48_C0D4.SAFE"
            };

            foreach (var currentPath in paths)
            {
                var data = await SAFEParser.OpenDataSetAsync(currentPath);
                Assert.IsNotNull(data);
            }
        }
        
        [TestMethod]
        public void GetDescription()
        {
            var description = Abbreviations.GetDescription(Abbreviations.Abbreviation.IPF);
        }

        [TestMethod]
        public void GetPropertyDescription()
        {
            var temp = new CalibriationAnnotation
            {
                AdsHeader = new AdsHeader()
            };

            if (MethodHelper.TryGetPropertyDescription(temp.AdsHeader.GetType(), "ProductType", out var description) == false)
            {

            }

            if(MethodHelper.TryGetPropertyDescriptions(temp.GetType(), out var output))
            {

            }
         }
    }
}