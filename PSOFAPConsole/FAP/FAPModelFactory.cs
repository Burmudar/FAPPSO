using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP
{
    public class FAPModelFactory
    {
        public FAPModelFactory() { }

        public FAPModel CreateModel(String path)
        {
            FAPModel model = new FAPModel(path);
            model.createModel();
            return model;
        }

        public FAPModel CreateSiemens1Model()
        {
            return CreateModel(@"..\..\FAP\Problems\siemens1");
        }

        public FAPModel CreateSiemens2Model()
        {
            return CreateModel(@"..\..\FAP\Problems\siemens2");
        }

        public FAPModel CreateSiemens3Model()
        {
            return CreateModel(@"..\..\FAP\Problems\siemens3");
        }

        public FAPModel CreateSiemens4Model()
        {
            return CreateModel(@"..\..\FAP\Problems\siemens4");
        }
    }
}
