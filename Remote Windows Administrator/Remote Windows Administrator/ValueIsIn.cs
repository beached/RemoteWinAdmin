using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RemoteWindowsAdministrator {
	public class ValueIsIn {
		private string _valueToCheck;
		public string ValueToCheck { set { _valueToCheck = value.ToUpperInvariant( ); } }
		public bool IsContained { get; private set; }

		public ValueIsIn( string value ) {
			IsContained = false;
			_valueToCheck = value.ToUpperInvariant( );
		}
	
		public ValueIsIn Test( string value ) {
			if( !string.IsNullOrEmpty( value ) ) {
				IsContained |= value.ToUpperInvariant( ).Contains( _valueToCheck );
			}
			return this;
		}

		public ValueIsIn Test( DateTime? value, string format = @"yyyy-MM-dd" ) {
			return null != value ? Test( value.Value.ToString( format, CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Test( double? value ) {
			return null != value ? Test( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Test( float? value ) {
			return null != value ? Test( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Test( bool? value ) {
			return null != value ? Test( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Test( long? value ) {
			return null != value ? Test( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Test( int? value ) {
			return null != value ? Test( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public void Reset( ) {
			IsContained = false;
		}
	}
}
