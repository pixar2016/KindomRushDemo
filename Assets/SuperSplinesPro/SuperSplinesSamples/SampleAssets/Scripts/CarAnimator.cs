using UnityEngine;

[AddComponentMenu("")]
public class CarAnimator : MonoBehaviour 
{
	public Spline spline;
	
	public WrapMode wrapMode = WrapMode.Clamp;
	
	public float speed = 1f;
	
	public float passedTime = 0f;
	
	public float rotationOffset;
	
	void Update( ) 
	{
		passedTime += Time.deltaTime * speed;
		
		float clampedParam = WrapValue( passedTime, 0f, 1f, wrapMode );
		
		transform.rotation = spline.GetOrientationOnSpline( WrapValue( passedTime + rotationOffset, 0f, 1f, wrapMode ) );
		transform.position = spline.GetPositionOnSpline( clampedParam ) - transform.right * spline.GetCustomValueOnSpline( clampedParam ) * .5f;
	}
	
	private float WrapValue( float v, float start, float end, WrapMode wMode )
	{
		switch( wMode )
		{
		case WrapMode.Clamp:
		case WrapMode.ClampForever:
			return Mathf.Clamp( v, start, end );
		case WrapMode.Default:
		case WrapMode.Loop:
			return Mathf.Repeat( v, end - start ) + start;
		case WrapMode.PingPong:
			return Mathf.PingPong( v, end - start ) + start;
		default:
			return v;
		}
	}
}
