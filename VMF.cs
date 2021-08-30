using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VMF_Viewer
{
    public class VMF
    {
        public static VMF instance;

        public VMF()
        {
            instance = this;
        }
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
        public void DeleteVMFMessage()
        {
            VMFMsg_delete_all();
        }
        #region VMF Data
        public byte[] BuildVMF(string originURN, List<string>destinationURN)
        {
            const int SUCCESS = 1;
            const int YES = 1;

            int id, datasize;
            uint n;
            String key;
            StringBuilder s = new StringBuilder(256);

            byte[] vmfbuf = new byte[10000];

            try
            {
                id = VMFMsg_create();
                VMFMsg_get_dll_version(id, s, s.Capacity);

                //Console.WriteLine("The current DLL version is \"{0}\"\n", s.ToString());
                Console.instance?.Write(String.Format("The current DLL version is \"{0}\"\n", s.ToString()));

                VMFMsg_set_options(id, "-desc"); // this will add comments to the parse output

                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.instance?.Write(s.ToString());
                    throw new Exception("exit program");
                }

                #region stuff
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
                #endregion

                /*
                VMFMsg_enter_uint(id, "hdr", "n8001_6_1", 2U); // VMF Version
                VMFMsg_enter_uint(id, "hdr", "g1.n4004_12_1", 1600205U); // Originator
                VMFMsg_enter_uint(id, "hdr", "g2.r1(1).n4004_12_2", 1600210U); // Recipient
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_7_1", 2U); // User Message Format Codes
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_8_1", 6U); // Message Standard Version
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_4_1", 1U); // Functional area designator
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8005_1_1", 1U); // Message Number
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_9_1", 0U); // Message Subtype
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_5_1", 1U); // Operation Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8007_4_1", 0U); // Retransmit Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_1_1", 6U); // Message Precedence Code
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_2_1", 0U); // Security Classification
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4098_1_1", 5U); // Year
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4099_1_1", 7U); // Month
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4019_1_1", 3U); // Day of Month
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n792_1_1", 7U); // Hour
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n797_4_1", 59U); // Minute
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n380_1_1", 30U); // Second
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_1_1", 0U); // Machine Ack Request Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_2_1", 0U); // Operator Ack Request Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).g7.n8007_3_1", 0U); // Operator Reply Request Indicator
                VMFMsg_enter_str(id, "bdy", "a4075_19_1", "FREE TEXT MESSAGE");  // Subject Line
                VMFMsg_enter_str(id, "bdy", "r1(1).a4075_1_1", "This is a test of the free text message."); // Message
                VMFMsg_enter_str(id, "bdy", "r1(2).a4075_1_1", "The purpose of a free text message is to provide information that does not fall into a structured format."); // Message 2
                */

                //1671A  //47001C
                // URNs                
                VMFMsg_enter_uint(id, "hdr", "n8001_6_1", 2U); // VMF Version
                VMFMsg_enter_uint(id, "hdr", "n4045_1_1", 1U); // GRI
                VMFMsg_enter_uint(id, "hdr", "g1.n4004_12_1", Convert.ToUInt32(originURN)); // Originator
                VMFMsg_enter_uint(id, "hdr", "g2.r1(1).n4004_12_2", 1600210U); // Recipient

                //for (int i = 0; i < destinationURN.Count; i++)
                //{
                 //   VMFMsg_enter_uint(id, "hdr", "g2.r1(" + i +").n4004_12_2", Convert.ToUInt32(destinationURN[i])); // Recipient
                //}

                VMFMsg_enter_str(id, "hdr", "r1.n281_402_1", "36"); // Source URN Lat
                VMFMsg_enter_str(id, "hdr", "r1.n282_402_1", "-86"); // Source URN Lon

                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_7_1", 2U); // User Message Format Codes
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_8_1", 6U); // Message Standard Version
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_4_1", 1U); // Functional area designator
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8005_1_1", 1U); // Message Number
                VMFMsg_enter_uint(id, "hdr", "r3(1).g4.n8001_9_1", 0U); // Message Subtype
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8001_5_1", 1U); // Operation Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8007_4_1", 0U); // Retransmit Indicator
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_1_1", 6U); // Message Precedence Code
                VMFMsg_enter_uint(id, "hdr", "r3(1).n8002_2_1", 0U); // Security Classification

                DateTime dateTime = DateTime.Now;                
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4098_1_1", (uint)dateTime.Year % 100); // Year
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4099_1_1", (uint)dateTime.Month); // Month
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n4019_1_1", (uint)dateTime.Day); // Day of Month
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n792_1_1", (uint)dateTime.Hour); // Hour
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n797_4_1", (uint)dateTime.Minute); // Minute
                VMFMsg_enter_uint(id, "hdr", "r3(1).g5.n380_1_1", (uint)dateTime.Second); // Second

                // Course/Speed Data
                //VMFMsg_enter_uint(id, "hdr", "r1.n4014_1_1", 1U); // GRI
                VMFMsg_enter_uint(id, "hdr", "g1.r1.n371_15_1", 2U); // Course
                VMFMsg_enter_uint(id, "hdr", "g1.r1.n367_15_1", 2U); // Speed (KPH)

                // MSG
                VMFMsg_enter_str(id, "bdy", "a4075_19_1", "SUBJECT LINE");
                VMFMsg_enter_str(id, "bdy", "r1(1).a4075_1_1", "Bruh 1.");
                VMFMsg_enter_str(id, "bdy", "r1(2).a4075_1_1", "Bruh 2.");

                VMFMsg_encode(id, vmfbuf, vmfbuf.Length, out datasize);
                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.instance?.Write("ERROR: " + s.ToString());
                }
            }
            catch (Exception e)
            {
                Console.instance?.Write(e.Message);
            }

            Console.instance?.Write("VMF Message Encoded");

            return vmfbuf;
        }
        public void ParseVMF(byte[] data)
        {
            const int SUCCESS = 1;
            const int YES = 1;
            int id, datasize;
            uint n;
            String key;
            StringBuilder s = new StringBuilder(256);

            byte[] vmfbuf = data;
            Console.instance?.Write(Encoding.Default.GetString(vmfbuf));

            try
            {
                id = VMFMsg_create();

                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.instance?.Write(s.ToString());
                    throw new Exception("exit program");
                }

                VMFMsg_get_dll_version(id, s, s.Capacity);

                VMFMsg_decode(id, vmfbuf, vmfbuf.Length);

                //Console.WriteLine("The current DLL version is \"{0}\"\n", s.ToString());
                Console.instance?.Write(String.Format("The current DLL version is \"{0}\"\n", s.ToString()));

                Console.instance?.Write(String.Format("Header size is " + VMFMsg_get_header_size(id) + " bytes"));

                //VMFMsg_set_options(id, "-desc"); // this will add comments to the parse output

                if (VMFMsg_have_errors(id) == YES)
                {
                    VMFMsg_error_getmsg(id, s, s.Capacity);
                    Console.instance?.Write(s.ToString());
                    throw new Exception("exit program");
                }                               

                VMFMessage message = new VMFMessage();

                Console.instance?.Write(vmfbuf.ToString());

                uint version = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "n8001_6_1", out version) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Version) FAILED!");

                uint origin = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "g1.n4004_12_1", out origin) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(OriginURN) FAILED!");

                uint destination = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "g2.r1(1).n4004_12_2", out destination) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(DestinationURN) FAILED!");


                //key = "r1.n281_402_1";
                //if (VMFMsg_decode_get_str(id, "hdr", key, s, s.Capacity) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_str(Lat) FAILED!");
                //Console.instance?.Write("\nKEY = " + key + ", VALUE = \"" + s.ToString() + "\"");
                /*
                uint lat = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r1.n281_402_1", out lat) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Lat) FAILED!");

                uint lon = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r1.n282_402_1", out lon) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Lon) FAILED!");
                */

                uint year = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n4098_1_1", out year) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Year) FAILED!");
                uint month = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n4099_1_1", out month) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Month) FAILED!");
                uint dom = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n4019_1_1", out dom) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(DoM) FAILED!");
                uint hour = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n792_1_1", out hour) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Hour) FAILED!");
                uint minute = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n797_4_1", out minute) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Minute) FAILED!");
                uint second = 0;
                if (VMFMsg_decode_get_uint(id, "hdr", "r3(1).g5.n380_1_1", out second) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Second) FAILED!");

                //uint course = 0;
                //if (VMFMsg_decode_get_uint(id, "hdr", "g1.r1.n371_15_1", out course) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Course) FAILED!");
                //uint speed = 0;
                //if (VMFMsg_decode_get_uint(id, "hdr", "g1.r1.n367_15_1", out speed) != SUCCESS) throw new Exception("FATAL ERROR - VMFMsg_decode_get_uint(Speed) FAILED!");

                message.Version = version;
                message.OriginURN = origin;
                message.DestinationURN = destination;
                //message.Lat = lat;
                //message.Lon = lon;
                message.Year = year;
                message.Month = month;
                message.Day_of_Month = dom;
                message.Hour = hour;
                message.Minute = minute;
                message.Second = second;
                //message.Course = course;
                //message.Speed = speed;

                Console.instance?.Write(message.ToString());
            }
            catch (Exception e)
            {
                Console.instance?.Write(e.Message);
            }

            VMFMsg_delete_all();

            Console.instance?.Write("\nPress any key to continue . . .");
        }
        #endregion
    }
}

public class VMFMessage
{
    public uint Version { get; set; }
    public uint OriginURN { get; set; }
    public uint DestinationURN { get; set; }
    public uint Lat { get; set; }
    public uint Lon { get; set; }

    public uint Year { get; set; }
    public uint Month { get; set; }
    public uint Day_of_Month { get; set; }
    public uint Hour { get; set; }
    public uint Minute { get; set; }
    public uint Second { get; set; }

    public uint Course { get; set; }
    public uint Speed { get; set; }

    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        s.AppendLine("Version: " + this.Version);
        s.AppendLine("Origin: " + this.OriginURN);
        s.AppendLine("Destination: " + this.DestinationURN);
        s.AppendLine("Latitude: " + this.Lat);
        s.AppendLine("Longitude: " + this.Lon);
        s.AppendLine("Year: " + this.Year);
        s.AppendLine("Month: " + this.Month);
        s.AppendLine("Day of Month: " + this.Day_of_Month);
        s.AppendLine("Hour: " + this.Hour);
        s.AppendLine("Minute: " + this.Minute);
        s.AppendLine("Second: " + this.Second);

        return s.ToString();
    }
}
