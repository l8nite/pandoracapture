using Fiddler;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

[assembly: Fiddler.RequiredVersion("2.3.5.0")]

namespace PandoraCapture
{
    public class PandoraCapture : IAutoTamper
    {
        public void AutoTamperResponseBefore(Session oSession)
        {
            if (oSession.uriContains("pandora") && oSession.uriContains(".mp3"))
            {
                string desktopPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                string suggestedFileName = Path.GetFileNameWithoutExtension(oSession.SuggestedFilename);
                string filePath = Path.Combine(desktopPath, suggestedFileName);

                while (File.Exists(filePath)) {
                    string name = Path.GetFileNameWithoutExtension(suggestedFileName);
                    string ext = Path.GetExtension(suggestedFileName);
                    name += "_";
                    filePath = Path.Combine(desktopPath, Path.ChangeExtension(name, ext));
                }

                File.WriteAllBytes(filePath, oSession.responseBodyBytes);
            }

            return;
        }

        public void OnLoad() {}
        public void OnBeforeUnload() {}
        public void AutoTamperResponseAfter(Session oSession) {}
        public void AutoTamperRequestBefore(Session oSession) { }
        public void AutoTamperRequestAfter(Session oSession) { }
        public void OnBeforeReturningError(Session oSession) { }
    }
}
