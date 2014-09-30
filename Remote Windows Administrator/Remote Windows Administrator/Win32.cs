using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace RemoteWindowsAdministrator {
	public static class Win32 {
		public static readonly Dictionary<string, string> WellKnownSids = new Dictionary<string, string> {
			{ @"S-1-0-0", @"NULL" }, 
			{ @"S-1-1-0", @"EVERYONE" }, 
			{ @"S-1-2-0", @"LOCAL" }, 
			{ @"S-1-2-1", @"CONSOLE_LOGON" }, 			
			{ @"S-1-3-0", @"CREATOR_OWNER" }, 
			{ @"S-1-3-1", @"CREATOR_GROUP" }, 
			{ @"S-1-3-2", @"OWNER_SERVER"  }, 
			{ @"S-1-3-3", @"GROUP_SERVER" },
			{ @"S-1-3-4", @"OWNER_RIGHTS" }, 
			{ @"S-1-5", @"NT_AUTHORITY" }, 
			{ @"S-1-5-1", @"DIALUP" }, 
			{ @"S-1-5-2", @"NETWORK" }, 
			{ @"S-1-5-3", @"BATCH" }, 
			{ @"S-1-5-4", @"INTERACTIVE" }, 
			{ @"S-1-5-6", @"SERVICE" }, 
			{ @"S-1-5-7", @"ANONYMOUS" }, 
			{ @"S-1-5-8", @"PROXY" }, 
			{ @"S-1-5-9", @"ENTERPRISE_DOMAIN_CONTROLLERS" }, 
			{ @"S-1-5-10", @"PRINCIPAL_SELF" }, 
			{ @"S-1-5-11", @"AUTHENTICATED_USERS" }, 
			{ @"S-1-5-12", @"RESTRICTED_CODE" }, 
			{ @"S-1-5-13", @"TERMINAL_SERVER_USER" }, 
			{ @"S-1-5-14", @"REMOTE_INTERACTIVE_LOGON" }, 
			{ @"S-1-5-15", @"THIS_ORGANIZATION" }, 
			{ @"S-1-5-17", @"IUSR" }, 
			{ @"S-1-5-18", @"LOCAL_SYSTEM" }, 
			{ @"S-1-5-19", @"LOCAL_SERVICE" }, 
			{ @"S-1-5-20", @"NETWORK_SERVICE" }
		};

		public enum SidNameUse {
			User = 1,
			Group,
			Domain,
			Alias,
			WellKnownGroup,
			DeletedAccount,
			Invalid,
			Unknown,
			Computer
		}

		[DllImport( "advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern bool LookupAccountSid( string lpSystemName, [MarshalAs( UnmanagedType.LPArray )] byte[] sid, System.Text.StringBuilder lpName, ref uint cchName, System.Text.StringBuilder referencedDomainName, ref uint cchReferencedDomainName, out SidNameUse peUse );

		/// <summary>
		/// The NetSessionEnum function provides information about sessions established on a server.
		/// </summary>
		/// <param name="serverName">[in] Pointer to a string that specifies the DNS or NetBIOS name of the remote server on which the function is to execute. If this parameter is NULL, the local computer is used.</param>
		/// <param name="clientName">[in] Pointer to a string that specifies the name of the computer session for which information is to be returned. If this parameter is NULL, NetSessionEnum returns information for all computer sessions on the server.</param>
		/// <param name="userName">[in] Pointer to a string that specifies the name of the user for which information is to be returned. If this parameter is NULL, NetSessionEnum returns information for all users.</param>
		/// <param name="level">[in] Specifies the information level of the data. This parameter can be one of the following values.
		/// <list>
		/// <listheader></listheader>
		/// <item>0 - Return the name of the computer that established the session. The buffer parameter points to an array of SESSION_INFO_0 structures. </item>
		/// <item>1 - Return the name of the computer, name of the user, and open files, pipes, and devices on the computer. The buffer parameter points to an array of SESSION_INFO_1 structures. </item>
		/// <item>2 - In addition to the information indicated for level 1, return the type of client and how the user established the session. The buffer parameter points to an array of SESSION_INFO_2 structures. </item>
		/// <item>10 - Return the name of the computer, name of the user, and active and idle times for the session. The buffer parameter points to an array of SESSION_INFO_10 structures. </item>
		/// <item>502 - Return the name of the computer; name of the user; open files, pipes, and devices on the computer; and the name of the transport the client is using. The buffer parameter points to an array of SESSION_INFO_502 structures. </item>
		/// </list>
		/// </param>
		/// <param name="buffer">
		/// [out] Pointer to the buffer that receives the data. The format of this data depends on the value of the level parameter.
		/// This buffer is allocated by the system and must be freed using the NetApiBufferFree function. Note that you must free the buffer even if the function fails with ERROR_MORE_DATA.
		/// </param>
		/// <param name="maxPrefferedLength">[in] Specifies the preferred maximum length of returned data, in bytes. If you specify MAX_PREFERRED_LENGTH, the function allocates the amount of memory required for the data. If you specify another value in this parameter, it can restrict the number of bytes that the function returns. If the buffer size is insufficient to hold all entries, the function returns ERROR_MORE_DATA.</param>
		/// <param name="entriesRead">[out] Pointer to a value that receives the count of elements actually enumerated.</param>
		/// <param name="totalEntries">[out] Pointer to a value that receives the total number of entries that could have been enumerated from the current resume position. Note that applications should consider this value only as a hint.</param>
		/// <param name="resumeHandle">[in, out] Pointer to a value that contains a resume handle which is used to continue an existing session search. The handle should be zero on the first call and left unchanged for subsequent calls. If resume_handle is NULL, no resume handle is stored.</param>
		/// <returns>If the function succeeds, the return value is Success.
		/// If the function fails, the return value can be one of the following error codes.
		/// <list>
		/// <item>ERROR_ACCESS_DENIED - The user does not have access to the requested information.</item>
		/// <item>ERROR_INVALID_LEVEL - The value specified for the level parameter is invalid. </item>
		/// <item>ERROR_INVALID_PARAMETER - The specified parameter is invalid. </item>
		/// <item>ERROR_MORE_DATA - More entries are available. Specify a large enough buffer to receive all entries. </item>
		/// <item>ERROR_NOT_ENOUGH_MEMORY - Insufficient memory is available. </item>
		/// <item>NERR_ClientNameNotFound - A session does not exist with the computer name. </item>
		/// <item>NERR_InvalidComputer - The computer name is invalid. </item>
		/// <item>NERR_UserNotFound - The user name could not be found. </item>
		/// </list>
		/// </returns>
		/// <remarks></remarks>
		[DllImport( "netapi32.dll", SetLastError = true )]
		public static extern int NetSessionEnum( [In, MarshalAs( UnmanagedType.LPWStr )] string serverName, [In, MarshalAs( UnmanagedType.LPWStr )] string clientName, [In, MarshalAs( UnmanagedType.LPWStr )] string userName, Int32 level, out IntPtr buffer, int maxPrefferedLength, ref Int32 entriesRead, ref Int32 totalEntries, ref Int32 resumeHandle );

		[StructLayout( LayoutKind.Sequential )]
		public struct SessionInfo502 {
			/// <summary>
			/// Unicode string specifying the name of the computer that established the session.
			/// </summary>
			[MarshalAs( UnmanagedType.LPWStr )]
			public string computerName;
			/// <summary>
			/// <value>Unicode string specifying the name of the user who established the session.</value>
			/// </summary>
			[MarshalAs( UnmanagedType.LPWStr )]
			public string userName;
			/// <summary>
			/// <value>Specifies the number of files, devices, and pipes opened during the session.</value>
			/// </summary>
			public uint numberOpenHandles;
			/// <summary>
			/// <value>Specifies the number of seconds the session has been active. </value>
			/// </summary>
			public uint logonDuration;
			/// <summary>
			/// <value>Specifies the number of seconds the session has been idle.</value>
			/// </summary>
			public uint ideTime;
			/// <summary>
			/// <value>Specifies a value that describes how the user established the session.</value>
			/// </summary>
			public uint userFlags;
			/// <summary>
			/// <value>Unicode string that specifies the type of client that established the session.</value>
			/// </summary>
			[MarshalAs( UnmanagedType.LPWStr )]
			public string clientType;
			/// <summary>
			/// <value>Specifies the name of the transport that the client is using to communicate with the server.</value>
			/// </summary>
			[MarshalAs( UnmanagedType.LPWStr )]
			public string transport;
		}

		public enum Error {
			/// <summary>
			/// Operation was a success.
			/// </summary>
			Success = 0,
			/// <summary>
			/// Security context does not have permission to make this call.
			/// </summary>
			ErrorAccessDenied = 5,
			/// <summary>
			/// Out of memory.
			/// </summary>
			ErrorNotEnoughMemory = 8,
			/// <summary>
			/// Network Path not found.
			/// </summary>
			ErrorBadNetpath = 53,
			/// <summary>
			/// Unable to contact resource. Connection timed out.
			/// </summary>
			ErrorNetworkBusy = 54,
			/// <summary>
			/// Parameter was incorrect.
			/// </summary>	
			ErrorInvalidParameter = 87,
			ErrorInsufficientBuffer = 122,
			/// <summary>
			/// LEVEL specified is not valid for this call.
			/// </summary>
			ErrorInvalidLevel = 124,	
			/// <summary>
			/// More data available to read. dderror getting all data.
			/// </summary>
			ErrorMoreData = 234,
			/// <summary>
			/// Extended Error.
			/// </summary>
			ErrorExtendedError = 1208,
			/// <summary>
			/// No available network connection to make call.
			/// </summary>
			ErrorNoNetwork = 1222,
			/// <summary>
			/// Pointer is not valid.
			/// </summary>
			ErrorInvalidHandleState = 1609,
			/// <summary>
			/// Base.
			/// </summary>
			NerrBase = 2100,
			/// <summary>
			/// Unknown Directory.
			/// </summary>
			NerrUnknownDevDir = (NerrBase + 16),
			/// <summary>
			/// Duplicate Share already exists on server.
			/// </summary>
			NerrDuplicateShare = (NerrBase + 18),
			/// <summary>
			/// Memory allocation was to small.
			/// </summary>
			NerrBufTooSmall = (NerrBase + 23),
			/// <summary>
			/// Network browsers not available.
			/// </summary>
			ErrorNoBrowserServersFound = 6118
		}

		/// <summary>
		/// The NetApiBufferFree function frees the memory that the NetApiBufferAllocate function allocates. Applications should also call NetApiBufferFree to free the memory that other network management functions use internally to return information.
		/// </summary>
		/// <returns>If the function succeeds, the return value is NERR_Success.
		/// If the function fails, the return value is a system error code</returns>
		[DllImport( "Netapi32.dll", SetLastError = true )]
		public static extern int NetApiBufferFree( IntPtr buffer );

		public static byte[] StringToBinarySid( string sid ) {
			SecurityIdentifier si;
			try {
				si = new SecurityIdentifier( sid );
			} catch( ArgumentException ex ) {
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, "Exception while looking up SID\n{0}", ex.Message );
				return null;
			}
			var binSid = new byte[si.BinaryLength];
			si.GetBinaryForm( binSid, 0 );
			return binSid;
		}
	}
}
