using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactNative.Bridge;
using ReactNative.Collections;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Globalization;

namespace io.fixd.rctlocale
{
    public class RCTLocaleModule : ReactContextNativeModuleBase
    {
        private ReactContext mContext;

        public RCTLocaleModule(ReactContext reactContext) : base(reactContext){
            mContext = reactContext;
        }

        public override string Name
        {
            get
            {
                return "RCTLocale";
            }
        }

        public override IReadOnlyDictionary<string, object> Constants
        {
            get
            {
                CultureInfo current = getLocale();
                NumberFormatInfo numberFormatSymbols = current.NumberFormat;

                Dictionary<String, Object> constants = new Dictionary<String, Object>();
                constants["localeIdentifier"] = current.ToString();
                constants["decimalSeparator"] = numberFormatSymbols.NumberDecimalSeparator;
                constants["groupingSeparator"] = numberFormatSymbols.NumberDecimalSeparator;

                Dictionary<String, String> formats = new Dictionary<String, String>();
                DateTimeFormatInfo dateFormatter = current.DateTimeFormat;
                formats["full"] = dateFormatter.FullDateTimePattern;
                formats["long"] = dateFormatter.LongDatePattern;
                formats["medium"] = dateFormatter.ShortDatePattern;
                formats["short"] = dateFormatter.ShortDatePattern;
                constants["localeDateFormats"] = formats;

                return constants;
            }
        }

        [ReactMethod]
        public void decimalStyle(Double number, IPromise promise)
        {
            try
            {
                String specifier = "G";
                promise.Resolve(number.ToString(specifier, getLocale()));
            }
            catch (Exception e)
            {
                promise.Reject(e.Message);
            }
        }

        [ReactMethod]
        public void numberFromDecimalString(String numberString, IPromise promise)
        {
            try
            {
                promise.Resolve(Double.Parse(numberString,getLocale()));
            }
            catch (Exception e)
            {
                promise.Reject(e.Message);
            }
        }

        [ReactMethod]
        public void dateFormat(String timestamp, String dateStyle, String timeStyle, IPromise promise)
        {
            try
            {
                DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                DateTime date = epochTime.AddSeconds(Convert.ToInt64(timestamp)).ToLocalTime();

                String dateTimeSpecifier = this.getDateTimeSpecifierFromString(dateStyle, timeStyle);

                if (dateTimeSpecifier != "none")
                {
                    promise.Resolve(date.ToString(dateTimeSpecifier, getLocale()));
                }
                else
                {
                    promise.Reject("DateFormat and TimeFormat cannot both be 'none'");
                }

            }
            catch (Exception e)
            {
                promise.Reject(e.Message);
            }
        }

        private String getDateTimeSpecifierFromString(String dateStyle, String timeStyle)
        {
            String dateFormatSpecifier;
            switch (dateStyle)
            {
                case "full":
                    dateFormatSpecifier = "f";
                    break;
                case "long":
                    dateFormatSpecifier = "f";
                    break;
                case "medium":
                    dateFormatSpecifier = "f";
                    break;
                case "short":
                    dateFormatSpecifier = "g";
                    break;
                case "none":
                    dateFormatSpecifier = "t";
                    break;
                default:
                    dateFormatSpecifier = "f";
                    break;
            }

            if (timeStyle != "short")
            {
                dateFormatSpecifier = dateFormatSpecifier.ToUpper();
            }

            if (timeStyle == "none")
            {
                if (dateStyle == "none")
                {
                    dateFormatSpecifier = "none";
                }
                else if (dateStyle == "short")
                {
                    dateFormatSpecifier = "d";
                }
                else
                {
                    dateFormatSpecifier = "D";
                }
            }

            return dateFormatSpecifier;
        }

        private CultureInfo getLocale()
        {
            CultureInfo current = CultureInfo.CurrentCulture;
            return current;
        }


    }
}
