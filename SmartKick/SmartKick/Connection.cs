using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;


namespace SmartKick
{

    public class Connection
    {

        private int tibiaPort = 7171;




        [DllImport("iphlpapi.dll")]
        private static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll")]
        private static extern int SetTcpEntry(IntPtr pTcprow);

        [DllImport("wsock32.dll")]
        private static extern int ntohs(int netshort);

        [DllImport("wsock32.dll")]
        private static extern int htons(int netshort);




        public Connection()
        {
            StatusChecker.HotkeyPress += DropConnection;
        }
        


        public void DropConnection()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {

                if (c.RemoteEndPoint.Port == tibiaPort)
                {
                    string[] ip = c.RemoteEndPoint.ToString().Split(':');                    
                    CloseRemoteIP(ip[0]);
                }
            }
        }


        private void CloseRemoteIP(string IP)
        {
            MIB_TCPROW[] rows = getTcpTable();
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].dwRemoteAddr == IPStringToInt(IP))
                {
                    rows[i].dwState = (int)State.Delete_TCB;
                    IntPtr ptr = GetPtrToNewObject(rows[i]);
                    int ret = SetTcpEntry(ptr);
                }
            }
        }

        private int IPStringToInt(string IP)
        {
            if (IP.IndexOf(".") < 0) throw new Exception("Invalid IP address");
            string[] addr = IP.Split('.');
            if (addr.Length != 4) throw new Exception("Invalid IP address");
            byte[] bytes = new byte[] { byte.Parse(addr[0]), byte.Parse(addr[1]), byte.Parse(addr[2]), byte.Parse(addr[3]) };
            return BitConverter.ToInt32(bytes, 0);
        }



        private IntPtr GetPtrToNewObject(object obj)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }



        private MIB_TCPROW[] getTcpTable()
        {
            IntPtr buffer = IntPtr.Zero; bool allocated = false;
            try
            {
                int iBytes = 0;
                GetTcpTable(IntPtr.Zero, ref iBytes, false);
                buffer = Marshal.AllocCoTaskMem(iBytes); 

                allocated = true;

                GetTcpTable(buffer, ref iBytes, false);

                int structCount = Marshal.ReadInt32(buffer);

                IntPtr buffSubPointer = buffer; 
                buffSubPointer = (IntPtr)((int)buffer + 4); 

                MIB_TCPROW[] tcpRows = new MIB_TCPROW[structCount];

                
                MIB_TCPROW tmp = new MIB_TCPROW();
                int sizeOfTCPROW = Marshal.SizeOf(tmp);

                
                for (int i = 0; i < structCount; i++)
                {
                    tcpRows[i] = (MIB_TCPROW)Marshal.PtrToStructure(buffSubPointer, typeof(MIB_TCPROW));
                    buffSubPointer = (IntPtr)((int)buffSubPointer + sizeOfTCPROW);
                }

                return tcpRows;

            }
            catch (Exception ex)
            {
                throw new Exception("getTcpTable failed! [" + ex.GetType().ToString() + "," + ex.Message + "]");
            }
            finally
            {
                if (allocated) Marshal.FreeCoTaskMem(buffer);
            }
        }




        private struct MIB_TCPROW
        {
            public int dwState { get; set; }
            public int dwLocalAddr { get; set; }
            public int dwLocalPort { get; set; }
            public int dwRemoteAddr { get; set; }
            public int dwRemotePort { get; set; }
        }
        


        private enum State
        {            
            All,           
            Closed,           
            Listen,            
            Syn_Sent,            
            Syn_Rcvd,            
            Established,            
            Fin_Wait1,            
            Fin_Wait2,            
            Close_Wait,            
            Closing,            
            Last_Ack,            
            Time_Wait,            
            Delete_TCB,
        }



    }




}
