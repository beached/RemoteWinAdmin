using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace RemoteWindowsAdministrator {
	public class Logging: IDisposable {
		public enum LogSeverity {
			Debug = 0,
			Info,
			Warning,
			Error,
			Fatal
		}
		public enum OutType {
			ConsoleOut = 0,
			File
		}
		public OutType OutputType { get; private set; }
		public LogSeverity Filter { get; set; }
	
		public Logging( ) {
			_sink = Console.Out;			
			OutputType = OutType.ConsoleOut;
			ResetFilter( );
		}

		public Logging( FileInfo outputFile ) {
			
			_sink = outputFile.CreateText( );
// 				using( var newFile = outputFile.CreateText( ) ) {
// 					newFile.WriteLine( @"Logfile started" );
// 				}	
// 			}
// 			_sink = new StreamWriter( outputFile.FullName, true, Encoding.UTF8 );
			
			OutputType = OutType.File;
			ResetFilter( );		
		}
		
		public void WriteLine( LogSeverity severity, string message ) {
			Helpers.AssertNotNull( _sink, @"Sink cannot be null" );
			if( Filter > severity ) {
				return;
			}
			message = string.Format( @"{0}, {1}, {2}", DateTime.Now, severity, message );
			_sink.WriteLine( message );
			//_sink.Flush( );
		}

		public void WriteLine( LogSeverity severity, string format, params object[] values ) {
			var message = string.Format( format, values );
			WriteLine( severity, message );
		}

		public void ResetFilter( ) {
			Filter = LogSeverity.Info;
		}

		// Implementation		
		private TextWriter _sink;
		private bool _disposed;

		protected virtual void Dispose( bool disposing ) {
			if( _disposed ) {
				return;
			}
			if( disposing ) {
				if( OutType.File == OutputType && null != _sink ) {
					_sink.Flush( );
					_sink.Close( );
					_sink = null;
				}
			}
			_disposed = true;
			//base.Dispose( true );
		}

		public void Dispose( ) {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		~Logging( ) {
			Dispose( false );
		}
	}

	public static class GlobalLogging {		
		public static Logging.LogSeverity Filter { get { return _logger.Filter; } set { _logger.Filter = value; } }
		private static string _lock = string.Empty;

		public static void ResetFilter( ) {
			Helpers.AssertNotNull( _logger, @"Attempt to set filter on an unopened log" );
			_logger.ResetFilter( );
		}

		public static void Open( ) {
			lock( _lock ) {
				if( null == _logger ) {
					_logger = new Logging( );
				} else {
					Helpers.Assert( Logging.OutType.ConsoleOut == _logger.OutputType, @"Attempt to open two different types of Global Loggers" );
				}
			}
		}

		public static void Open( FileInfo outputFile ) {
			lock( _lock ) {
				if( null == _logger ) {
					_logger = new Logging( outputFile );
				} else {
					Helpers.Assert( Logging.OutType.File == _logger.OutputType, @"Attempt to open two different types of Global Loggers" );
				}
			}
		}

		public static void Close( ) {
			lock( _lock ) {
				if( null == _logger ) {
					return;
				}
				_logger.Dispose( );
				_logger = null;
			}
		}

		public static void WriteLine( Logging.LogSeverity severity, string message ) {
			// Do nothing if we have not been setup
			if( null == _logger ) {
				return;
			}
			_logger.WriteLine( severity, message );
		}

		public static void WriteLine( Logging.LogSeverity severity, string format, params object[] values ) {
			if( null == _logger ) {
				return;
			}
			_logger.WriteLine( severity, format, values );
		}

		private static Logging _logger;

	}
}
