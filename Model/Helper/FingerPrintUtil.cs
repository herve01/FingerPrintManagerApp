using DPFP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Model.Helper
{
    public class FingerPrintUtil
    {
        public static FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            FeatureSet features = new FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        public static bool verifiy(DPFP.Sample Sample, byte[] template)
        {
            var Verificator = new DPFP.Verification.Verification();
            DPFP.FeatureSet feature = FingerPrintUtil.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            var stream = new MemoryStream(template);
            DPFP.Template Dtemplate = new DPFP.Template(stream);
            //template.Serialize(stream);

            DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
            Verificator.Verify(feature, Dtemplate, ref result);

            return result.Verified;
        }

        public static Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();
            Bitmap bitmap = null;
            Convertor.ConvertToPicture(Sample, ref bitmap);
            return bitmap;
        }
    }
}
