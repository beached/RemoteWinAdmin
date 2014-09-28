using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace RemoteWindowsAdministrator {
	public class ValueIsIn {
		private string _valueToCheck;
		public string ValueToCheck { set { _valueToCheck = value.ToUpperInvariant( ); } }
		public bool IsContained { get; private set; }

		public ValueIsIn( string value ) {
			IsContained = false;
			_valueToCheck = value.ToUpperInvariant( );
		}
	
		public ValueIsIn Add( string value ) {
			if( !IsContained && !string.IsNullOrEmpty( value ) ) {
				IsContained |= value.ToUpperInvariant( ).Contains( _valueToCheck );
			}
			return this;
		}

		public ValueIsIn Add( IEnumerable<string> values ) {
			if( IsContained ) {
				return this;
			}
			foreach( var value in values ) {
				Add( value );
				if( IsContained ) {
					break;
				}
			}
			return this;
		}

		public ValueIsIn Add( DateTime? value, string format = MagicValues.ShortDateFormat ) {
			return !IsContained && null != value ? Add( value.Value.ToString( format, CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( double? value ) {
			return !IsContained && null != value ? Add( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( float? value ) {
			return !IsContained && null != value ? Add( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( bool? value ) {
			return !IsContained && null != value ? Add( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( long? value ) {
			return !IsContained && null != value ? Add( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( int? value ) {
			return !IsContained && null != value ? Add( value.Value.ToString( CultureInfo.InvariantCulture ) ) : this;
		}

		public ValueIsIn Add( ) {
			return this;
		}

		public ValueIsIn Add( object value ) {
			return !IsContained && null != value ? Add( value.ToString( ) ) : this;
		}

		public void Reset( ) {
			IsContained = false;
		}
	}
}
