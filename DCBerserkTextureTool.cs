using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCBerserkTextureTool
{
    public partial class DCBerserkTextureTool : Form
    {
        public DCBerserkTextureTool()
        {
            InitializeComponent();
        }

        public class TexFile
        {
            // Only for logic reasons (does not exist)
            public int OrderIndex;
            public string Filename;
            public string PathToExport;
            public int PvrImageDataPointer;

            // Tex File
            public byte[] Index;               // 0x00 - 0x03 - global-index-like id (like gbix)
            public byte[] PvrPixelFormat;      // 0x04 - pvr format settings byte 1
            public byte[] PvrDataFormat;       // 0x05 - pvr format settings byte 2
            public byte[] PvrWidth;            // 0x06 - 0x09 - size x
            public byte[] PvrHeight;           // 0x0a - 0x0d - size y
            public byte[] HeaderEnd;           // 0x0e - 0x0f - header end/reserved (0x00)
            public byte[] PvrImageData;        // 0x?? - 0x?? - image data
        }

        public void ButtonsEnabledState(bool state)
        {
            if (!state)
            {
                label_Working.Visible = true;
            }
            else
            {
                label_Working.Visible = false;
            }

            buttonPack.Enabled = state;
            buttonUnpack.Enabled = state;
            button_packSingle.Enabled = state;
            button_unpackSingle.Enabled = state;
        }

        public void UnPackTex(string[] files)
        {
            ButtonsEnabledState(false);
            var fileList = new List<TexFile>();

            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".TEX")
                {
                    var texFilePath = file;
                    // Create path for extracted PVRs
                    Directory.CreateDirectory(texFilePath + "_");

                    using (var stream = new FileStream(texFilePath, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            int fileCount = reader.ReadInt32();
                            long tempPos = reader.BaseStream.Position;

                            List<int> pointerList = new List<int>();

                            for (int i = 0; i < fileCount; i++)
                            {
                                var texPointer = reader.ReadInt32();
                                pointerList.Add(texPointer);
                            }

                            bool eof = false;
                            for (int j = 0; j < pointerList.Count; j++)
                            {
                                if (pointerList.Count <= (j + 1)) eof = true;
                                reader.BaseStream.Seek(pointerList[j], SeekOrigin.Begin);

                                var tmpPvr = new TexFile
                                {
                                    OrderIndex = j,
                                    Filename = Path.GetFileNameWithoutExtension(texFilePath),
                                    PathToExport = texFilePath,

                                    Index = reader.ReadBytes(4),
                                    PvrPixelFormat = reader.ReadBytes(1),
                                    PvrDataFormat = reader.ReadBytes(1),
                                    PvrWidth = reader.ReadBytes(4),
                                    PvrHeight = reader.ReadBytes(4),
                                    HeaderEnd = reader.ReadBytes(2),

                                    PvrImageDataPointer = (int)reader.BaseStream.Position,
                                };

                                if (eof)
                                {
                                    List<byte> tmpBuf = new List<byte>();
                                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                                    {
                                        var byteTmp = reader.ReadByte();
                                        tmpBuf.Add(byteTmp);
                                    }
                                    tmpPvr.PvrImageData = tmpBuf.ToArray();
                                }
                                else
                                {
                                    tmpPvr.PvrImageData = reader.ReadBytes(pointerList[j + 1] - pointerList[j]);
                                }

                                fileList.Add(tmpPvr);

                            }
                        }
                    }
                }
            }

            //export converted files
            foreach (var pvrFile in fileList)
            {
                var newPVRPath = pvrFile.PathToExport + "_\\" + pvrFile.Filename + "_" + pvrFile.OrderIndex + "_" + pvrFile.PvrImageDataPointer + ".PVR";

                using (BinaryWriter binWriter = new BinaryWriter(File.Open(newPVRPath, FileMode.Create)))
                {
                    //GBIX HEADER
                    binWriter.Write(new byte[] {
                                        0x47, 0x42, 0x49, 0x58, 0x08, 0x00, 0x00, 0x00,
                                        pvrFile.Index[0], pvrFile.Index[1], pvrFile.Index[2], pvrFile.Index[3], 0x00, 0x00, 0x00, 0x00 });
                    //PVRT Identifier
                    binWriter.Write(new byte[] {
                                        0x50, 0x56, 0x52, 0x54 });

                    // file size (+ 8 bytes - 4 bytes format param, 2 bytes width & 2 bytes height)
                    binWriter.Write(pvrFile.PvrImageData.Length + 8);

                    binWriter.Write(pvrFile.PvrPixelFormat);
                    binWriter.Write(pvrFile.PvrDataFormat);

                    //Compression (NONE/RLE) & reserved (is not found in TEX file, always NONE)
                    binWriter.Write(new byte[] {
                                        0x00, 0x00 });

                    //Width
                    binWriter.Write(new byte[] {
                                        pvrFile.PvrWidth[2], pvrFile.PvrWidth[3] });

                    //Height
                    binWriter.Write(new byte[] {
                                        pvrFile.PvrHeight[2], pvrFile.PvrHeight[3] });

                    //final output of pvr file
                    binWriter.Write(pvrFile.PvrImageData);
                }
            }
        }

        public void PackTex(string[] directories)
        {
            foreach (var texFolder in directories)
            {
                var texPath = texFolder.Remove(texFolder.Length - 1, 1);
                var pvrFiles = Directory.GetFiles(texFolder);
                using (BinaryWriter binWriter = new BinaryWriter(File.Open(texPath, FileMode.Open)))
                {
                    foreach (var pvrFile in pvrFiles)
                    {
                        //Filename, OrderIndex, PvrImageDataPointer
                        string[] pvrParams = Path.GetFileNameWithoutExtension(pvrFile).Split('_');

                        // Validation
                        if(pvrParams.Length < 3)
                        {
                            MessageBox.Show("Folder has a invalid name? - Packing will stop now.");
                            return;
                        }

                        // Some filenames have a underscore in it, so skip them over
                        int paramOrder = 0;
                        if (pvrParams.Length > 3)
                        {
                            paramOrder = pvrParams.Length - 3; //Filename, OrderIndex, PvrImageDataPointer
                        }

                        int pvrPointer = Convert.ToInt32(pvrParams[2 + paramOrder]);
                        binWriter.Seek(pvrPointer, SeekOrigin.Begin);
                        List<byte> tmpBuf = new List<byte>();
                        using (BinaryReader reader = new BinaryReader(new FileStream(pvrFile, FileMode.Open)))
                        {
                            //skip gbix/pvr header
                            reader.BaseStream.Seek(0x20, SeekOrigin.Begin);

                            // read pvr image data 
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                var byteTmp = reader.ReadByte();
                                tmpBuf.Add(byteTmp);
                            }
                        }

                        //override pvr image data with new bytes
                        binWriter.Write(tmpBuf.ToArray());
                    }
                }
            }
        }

        private void buttonUnpack_Click(object sender, EventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (fbd.SelectedPath.Contains("\\TEXTURE"))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath);
                        UnPackTex(files);
                    }
                    else
                    {
                        MessageBox.Show("Wrong folder selected! " + fbd.SelectedPath +
                            Environment.NewLine + Environment.NewLine + "Please select the DC Berserk folder that is named 'TEXTURE'");
                    }
                }
            }

            ButtonsEnabledState(true);
        }

        private void buttonPack_Click(object sender, EventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (fbd.SelectedPath.Contains("\\TEXTURE"))
                    {
                        ButtonsEnabledState(false);
                        string[] directories = Directory.GetDirectories(fbd.SelectedPath);
                        PackTex(directories);
                    }
                    else
                    {
                        MessageBox.Show("Wrong folder selected! " + fbd.SelectedPath +
                            Environment.NewLine + Environment.NewLine + "Please select the DC Berserk folder that is named 'TEXTURE'");
                    }
                }
            }
            ButtonsEnabledState(true);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_unpackSingle_Click(object sender, EventArgs e)
        {
            using (var fbd = new VistaOpenFileDialog())
            {
                fbd.Filter = "Berserk Texture File (*.TEX)|*.TEX";
                fbd.Multiselect = false;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (fbd.FileName.EndsWith(".TEX"))
                    {
                        string[] files = new string[1] { fbd.FileName };
                        UnPackTex(files);
                    }
                    else
                    {
                        MessageBox.Show("Wrong file selected! " + fbd.FileName +
                            Environment.NewLine + Environment.NewLine + "Please select a DC Berserk file that ends with .TEX");
                    }
                }
            }

            ButtonsEnabledState(true);
        }

        private void button_packSingle_Click(object sender, EventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (fbd.SelectedPath.EndsWith(".TEX_"))
                    {
                        ButtonsEnabledState(false);
                        string[] directories = new string[1] { fbd.SelectedPath };
                        PackTex(directories);
                    }
                    else
                    {
                        MessageBox.Show("Wrong folder selected! " + fbd.SelectedPath +
                        Environment.NewLine + Environment.NewLine + "Please select a DC Berserk folder that ends with '.TEX_'");
                    }
                }
            }
            ButtonsEnabledState(true);
        }
    }
}
