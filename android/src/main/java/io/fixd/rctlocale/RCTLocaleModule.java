package io.fixd.rctlocale;

import com.facebook.react.bridge.NativeModule;
import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ReactMethod;
import com.facebook.react.bridge.Promise;
import com.facebook.react.bridge.WritableNativeMap;

import java.util.Map;
import java.util.HashMap;
import java.util.Locale;
import java.text.DecimalFormatSymbols;
import java.text.NumberFormat;
import java.text.DecimalFormat;

public class RCTLocaleModule extends ReactContextBaseJavaModule {

    private ReactApplicationContext mContext;

    public RCTLocaleModule(ReactApplicationContext reactContext) {
        super(reactContext);
        mContext = reactContext;
    }

    @Override
    public String getName() {
        return "RCTLocale";
    }

    @Override
    public Map<String, Object> getConstants() {

        Locale current = getLocale();
        DecimalFormatSymbols formatterSymbols = getDecimalFormat().getDecimalFormatSymbols();

        final Map<String, Object> constants = new HashMap<>();
        constants.put("locale", current.toString());
        constants.put("decimalSeparator", String.valueOf(formatterSymbols.getDecimalSeparator()));
        constants.put("groupingSeparator", String.valueOf(formatterSymbols.getGroupingSeparator()));
        return constants;
    }

    @ReactMethod
    public void decimalStyle(Double number, Promise promise) {
        try {
            NumberFormat nf = NumberFormat.getNumberInstance(getLocale());
            nf.setGroupingUsed(true);

            WritableNativeMap result = new WritableNativeMap();
            result.putDouble("original", number);
            result.putString("result", nf.format(number));

            promise.resolve(result);
        } catch (Exception e) {
            e.printStackTrace();
            promise.reject(e);
        }
    }

    @ReactMethod
    public void numberFromDecimalString(String numberString, Promise promise) {
        try {
            NumberFormat nf = NumberFormat.getInstance(getLocale());

            WritableNativeMap result = new WritableNativeMap();
            result.putString("original", numberString);
            result.putDouble("result", nf.parse(numberString).doubleValue());

            promise.resolve(result);
        } catch (Exception e) {
            e.printStackTrace();
            promise.reject(e.getMessage());
        }
    }

    private Locale getLocale() {
        Locale current = mContext.getResources().getConfiguration().locale;
        return current;
    }

    private DecimalFormat getDecimalFormat() {
        DecimalFormat formatter = (DecimalFormat) DecimalFormat.getInstance(getLocale());
        return formatter;
    }
}