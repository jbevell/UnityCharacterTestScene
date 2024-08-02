using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuaternionUtility
{
	public static Quaternion FindRotationBetweenVectors(Vector3 v1, Vector3 v2)
	{
		Vector3 crossVector = Vector3.Cross(v1, v2);
		Debug.Log($"Rotation vector is {crossVector}");

		return Quaternion.Euler(crossVector);
	}

	public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
	{
		if (Quaternion.Dot(a, b) < 0)
		{
			return a * Quaternion.Inverse(Multiply(b, -1));
		}

		else return a * Quaternion.Inverse(b);
	}

	public static Quaternion Multiply(Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}

	public static string FormatAsQuaternionExpression(Quaternion quaternion)
	{
		return $"{quaternion.w} {FormatSignForAddOrSub(quaternion.x)}i {FormatSignForAddOrSub(quaternion.y)}j {FormatSignForAddOrSub(quaternion.z)}k";
	}

	public static string FormatSignForAddOrSub(float value, bool addSpace = true)
	{
		string operation = Mathf.Sign(value) > 0 ? "+" : "-";
		string space = addSpace ? " " : "";

		return $"{operation}{space}{Mathf.Abs(value).ToString("0.00")}";
	}
}
