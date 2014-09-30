using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace RemoteWindowsAdministrator {
	public class ValueIsIn {
		private readonly string _valueToCheck;
		private readonly bool _ignoreCase;

		public bool IsContained { get; private set; }

		public ValueIsIn( string value, bool ignoreCase = true ) {
			_valueToCheck = value;
			_ignoreCase = ignoreCase;
			IsContained = false;
			if( _ignoreCase ) {
				_valueToCheck = _valueToCheck.ToUpperInvariant( );
			}
		}
	
		public ValueIsIn Add( string value ) {
			if( IsContained || string.IsNullOrEmpty( value ) ) {
				return this;
			}
			if( _ignoreCase ) {
				IsContained |= value.ToUpperInvariant( ).Contains( _valueToCheck );
			} else {
				IsContained |= value.Contains( _valueToCheck );
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
