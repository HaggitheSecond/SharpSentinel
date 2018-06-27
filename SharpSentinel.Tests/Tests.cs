using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpSentinel.Parser;
using SharpSentinel.Parser.Resources;

namespace SharpSentinel.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task LoadSAFE()
        {
           // var path = @"C:\Users\Yannik\Desktop\SharpSentinel\doof\S1B_IW_GRDH_1SDV_20180511T052604_20180511T052629_010869_013E1D_F9FD.SAFE";
            var path = @"C:\Users\Yannik\Desktop\SharpSentinel\S1B_IW_GRDH_1SDV_20180606T050938_20180606T051003_011248_014A48_7E3C.SAFE";
            var data = await SAFEParser.OpenDataSetAsync(path);
            
            Assert.IsNotNull(data);
        }
        
        [TestMethod]
        public void GetDescription()
        {
            var description = Abbreviations.GetDescription(Abbreviations.Abbreviation.IPF);
        }
    }
}