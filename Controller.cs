using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMF_Viewer
{
    public partial class Controller : Form
    {

        #region Variables
        public enum OutputType { K051, K0519, K071};
        public OutputType outputType = OutputType.K051;
        public enum Mode { Sender, Reciever};
        public Mode mode = Mode.Sender;
        #endregion
        #region VMFImports
        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_create();

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_delete(int id);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_delete_all();

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_decode(int id, byte[] vmfbuf, int size);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_decode_getnext(int id, StringBuilder s, int size);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_decode_get_uint(int id, String tag, String key, out uint n);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_decode_get_str(int id, String tag, String key, StringBuilder s, int size);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_enter_uint(int id, String tag, String key, uint value);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_enter_str(int id, String tag, String key, String value);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_encode(int id, [MarshalAs(UnmanagedType.LPArray)] byte[] vmfbuf, int bufsize, out int datasize);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_have_errors(int id);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_error_getmsg(int id, StringBuilder s, int size);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_error_getnext(int id, StringBuilder s, int size);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_set_options(int id, String s);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VMFMsg_get_header_size(int id);

        [DllImport("VMFMessageDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VMFMsg_get_dll_version(int id, StringBuilder s, int size);
        #endregion
        #region VMF Data
        public void VMF()
        {
            const int SUCCESS = 1;
            const int YES = 1;

            int id, datasize;
            uint n;
            String key;
            StringBuilder s = new StringBuilder(256);

            String testvmb_filename = "z_k01_1_0_1_1.vmb";
            String testnew_filename = "z_k01_1_0_1_1_new.vmb";

            try
            {
                id = VMFMsg_create();
                VMFMsg_get_dll_version(id, s, s.Capacity);

                Console.WriteLine("The current DLL version is \"{0}\"\n", s.ToString());

                VMFMsg_set_options(id, "-desc"); // this will add comments to the parse output

                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.WriteLine(s.ToString());
                    throw new Exception("exit program");
                }
                /*
                String path = Directory.GetCurrentDirectory();
                Console.WriteLine("The current directory is\n{0}", path);

                if (!File.Exists(testvmb_filename)) throw new Exception("File \"" + testvmb_filename + "\" does not exist!!");

                byte[] vmfmsg = File.ReadAllBytes(testvmb_filename);             

                // --------------------------------------------------------------------------------
                // SHOW DECODE OF A .VMB LOADED INTO A BINARY BUFFER
                // valid values are "jbcp", "swb2", "swb3", "jcr", "6017B", "6017C"
                //--------------------------------------------------------------------------------

                VMFMsg_decode(id, vmfmsg, vmfmsg.Length);
                if (VMFMsg_have_errors(id) == YES)
                {
                    // VMFMsg_error_getmsg(id, s, s.Capacity);
                    while (VMFMsg_error_getnext(id, s, s.Capacity) == YES)
                    {
                        Console.WriteLine(s.ToString());
                    }
                    Console.WriteLine(s.ToString());
                    throw new Exception("exit program");
                }

                Console.WriteLine("Header size is " + VMFMsg_get_header_size(id) + " bytes");

                // --------------------------------------------------------------------------------
                // DEMONSTRATE USE OF THE ITERATOR TO DISPLAY MESSAGE CONTENTS
                // --------------------------------------------------------------------------------                
                while (VMFMsg_decode_getnext(id, s, s.Capacity) == YES)
                {
                    Console.WriteLine(s.ToString());
                }

                // --------------------------------------------------------------------------------
                // SHOW DIRECT ACCESS CAPABILITY TO GET A STRING OR AN UNSIGNED INTEGER
                // --------------------------------------------------------------------------------
                key = "g1.n4004_12_1";
                if (VMFMsg_decode_get_uint(id, "hdr", key, out n) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint( ) FAILED!");
                Console.WriteLine("\nKEY = " + key + ", VALUE = " + n);

                key = "r3(1).g5.n797_4_1";
                if (VMFMsg_decode_get_uint(id, "hdr", key, out n) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint( ) FAILED!");
                Console.WriteLine("\nKEY = " + key + ", VALUE = " + n);

                key = "a4075_19_1";
                if (VMFMsg_decode_get_str(id, "bdy", key, s, s.Capacity) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_str( ) FAILED!");
                Console.WriteLine("\nKEY = " + key + ", VALUE = \"" + s.ToString() + "\"");

                key = "r1(1).a4075_1_1";
                if (VMFMsg_decode_get_str(id, "bdy", key, s, s.Capacity) != SUCCESS)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    throw new Exception("FATAL ERROR - VMFMsg_decode_get_str( ) FAILED!\nVMFMsg ERROR: " + s.ToString());
                }
                Console.WriteLine("\nKEY = " + key + ", VALUE = \"" + s.ToString() + "\"");
                */
                // --------------------------------------------------------------------------------
                // EXAMPLE ENCODE A FREE TEXT MESSAGE
                // --------------------------------------------------------------------------------                

                /*
                VMFMsg_enter_uint(id, "hdr", "n8001_6_1", 2U);
                VMFMsg_enter_uint(id, "hdr", "g1.n4004_12_1", 1600205U);
                VMFMsg_enter_uint(id, "hdr", "g2.r1(1).n4004_12_2", 1600210U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_7_1", 2U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_8_1", 6U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_4_1", 1U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8005_1_1", 1U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_9_1", 0U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_5_1", 1U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8007_4_1", 0U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_1_1", 6U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_2_1", 0U); //1671A  //47001C
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4098_1_1", 5U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4099_1_1", 7U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4019_1_1", 3U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n792_1_1", 7U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n797_4_1", 59U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n380_1_1", 30U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_1_1", 0U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_2_1", 0U);
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_3_1", 0U);
                VMFMsg_enter_str(id, "bdy", "a4075_19_1", "FREE TEXT MESSAGE");
                VMFMsg_enter_str(id, "bdy", "r1(1).a4075_1_1", "This is a test of the free text message.");
                VMFMsg_enter_str(id, "bdy", "r1(2).a4075_1_1", "The purpose of a free text message is to provide information that does not fall into a structured format.");
                */

                VMFMsg_enter_uint(id, "hdr", "n8001_6_1", 2U);

                byte[] vmfbuf = new byte[10000];

                VMFMsg_encode(id, vmfbuf, vmfbuf.Length, out datasize);
                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.WriteLine("ERROR: " + s.ToString());
                }

                Array.Resize(ref vmfbuf, datasize);
                File.WriteAllBytes(testnew_filename, vmfbuf);

                byte[] A = File.ReadAllBytes(testvmb_filename);
                byte[] B = File.ReadAllBytes(testnew_filename);

                // --------------------------------------------------------------------------------
                // WE DECODED A VMF MESSAGE THEN ENCODED ONE USING THE SAME VALUES
                // --------------------------------------------------------------------------------                 

                if (A.SequenceEqual(B)) Console.WriteLine("\nYES, decode and encode are equal");
                else Console.WriteLine("\nNO, decode and encode are *NOT* equal");

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            VMFMsg_delete_all();

            Console.WriteLine("\nPress any key to continue . . .");
            //Console.ReadKey();
        }
        #endregion
        public Controller()
        {
            InitializeComponent();
            // Initalize Varibales
            this.OutputTypeDropdown.SelectedIndex = 1;
            this.ModeDropdown.SelectedIndex = 0;
            this.SendTimeInputBox.Text = "180";

            this.DestinationURNDataGridView.CellEndEdit += ValidateDesinationURNInput;

            VMF();            
        }
        #region Data Bindings
        private void ValidateDesinationURNInput(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            var name = this.DestinationURNDataGridView.Rows[rowIndex].Cells[0];
            var urn = this.DestinationURNDataGridView.Rows[rowIndex].Cells[1];
            if (name != null && urn.Value != null && urn.Value.ToString().Length > 0)
            {
                // Cells are validated
                Console.WriteLine("Cells Written");
                this.OriginatorURNInputBox.Text = urn.Value.ToString();
            }
        }

        private void OutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.outputType = (OutputType)Enum.ToObject(typeof(OutputType), this.OutputTypeDropdown.SelectedIndex);
        }

        private void ModeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mode = (Mode)Enum.ToObject(typeof(Mode), this.ModeDropdown.SelectedIndex);
        }
        #endregion
    }
}
