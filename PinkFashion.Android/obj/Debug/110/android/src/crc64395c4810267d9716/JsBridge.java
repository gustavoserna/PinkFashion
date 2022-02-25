package crc64395c4810267d9716;


public class JsBridge
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_InvokeAction:(Ljava/lang/String;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("PinkFashion.Droid.Renderers.JsBridge, PinkFashion.Android", JsBridge.class, __md_methods);
	}


	public JsBridge ()
	{
		super ();
		if (getClass () == JsBridge.class)
			mono.android.TypeManager.Activate ("PinkFashion.Droid.Renderers.JsBridge, PinkFashion.Android", "", this, new java.lang.Object[] {  });
	}

	public JsBridge (crc64395c4810267d9716.HybridWebViewRenderer p0)
	{
		super ();
		if (getClass () == JsBridge.class)
			mono.android.TypeManager.Activate ("PinkFashion.Droid.Renderers.JsBridge, PinkFashion.Android", "PinkFashion.Droid.Renderers.HybridWebViewRenderer, PinkFashion.Android", this, new java.lang.Object[] { p0 });
	}

	@android.webkit.JavascriptInterface

	public void invokeAction (java.lang.String p0)
	{
		n_InvokeAction (p0);
	}

	private native void n_InvokeAction (java.lang.String p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
