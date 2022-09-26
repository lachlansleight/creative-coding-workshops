//Takes in a position, rotation (quaternion) and scale,
//and outputs a 4x4 affine transformation matrix
//If you don't know what that is, be grateful.
float4x4 GetTransformMatrix(float3 p, float4 r, float3 s)
{
    //just has position for now
    float4x4 m = float4x4(1, 0, 0, p.x,
                          0, 1, 0, p.y,
                          0, 0, 1, p.z,
                          0, 0, 0, 1);
                                  
    //add rotation to the matrix
    //if you think I understand this at all, you are sorely mistaken
    //this part copied from https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/index.htm
    m._m00 = 1 - 2 * r.y * r.y - 2 * r.z * r.z;
    m._m01 = 2 * r.x * r.y - 2 * r.z * r.w;
    m._m02 = 2 * r.x * r.z + 2 * r.y * r.w;
    m._m10 = 2 * r.x * r.y + 2 * r.z * r.w;
    m._m11 = 1 - 2 * r.x * r.x - 2 * r.z * r.z;
    m._m12 = 2 * r.y * r.z - 2 * r.x * r.w;
    m._m20 = 2 * r.x * r.z - 2 * r.y * r.w;
    m._m21 = 2 * r.y * r.z + 2 * r.x * r.w;
    m._m22 = 1 - 2 * r.x * r.x - 2 * r.y * r.y;
          
    //add scale to the matrix
    m._m00_m01_m02 *= s.x;
    m._m10_m11_m12 *= s.y;
    m._m20_m21_m22 *= s.z;
            
    return m;
}

//Basically the equivalent of Quaternion.LookRotation in C#
//I also do not understand this at all :D
//from https://answers.unity.com/questions/467614/what-is-the-source-code-of-quaternionlookrotation.html
float4 lookRotation(float3 fwd, float3 up)
{         
    float3 vector2 = normalize(cross(up, fwd));
    float3 vector3 = cross(fwd, vector2);
    float3x3 m = float3x3(vector2.x, vector3.x, fwd.x,
                          vector2.y, vector3.y, fwd.y,
                          vector2.z, vector3.z, fwd.z);
 
    float num8 = (m._m00 + m._m11) + m._m22;
    float4 quaternion;
    if (num8 > 0)
    {
        float num = sqrt(num8 + 1);
        quaternion.w = num * 0.5;
        num = 0.5 / num;
        quaternion.x = (m._m12 - m._m21) * num;
        quaternion.y = (m._m20 - m._m02) * num;
        quaternion.z = (m._m01 - m._m10) * num;
        return quaternion;
    }
    if ((m._m00 >= m._m11) && (m._m00 >= m._m22))
    {
        float num7 = sqrt(((1 + m._m00) - m._m11) - m._m22);
        float num4 = 0.5 / num7;
        quaternion.x = 0.5 * num7;
        quaternion.y = (m._m01 + m._m10) * num4;
        quaternion.z = (m._m02 + m._m20) * num4;
        quaternion.w = (m._m12 - m._m21) * num4;
        return quaternion;
    }
    if (m._m11 > m._m22)
    {
        float num6 = sqrt(((1 + m._m11) - m._m00) - m._m22);
        float num3 = 0.5 / num6;
        quaternion.x = (m._m10+ m._m01) * num3;
        quaternion.y = 0.5 * num6;
        quaternion.z = (m._m21 + m._m12) * num3;
        quaternion.w = (m._m20 - m._m02) * num3;
        return quaternion; 
    }
    float num5 = sqrt(((1 + m._m22) - m._m00) - m._m11);
    float num2 = 0.5 / num5;
    quaternion.x = (m._m20 + m._m02) * num2;
    quaternion.y = (m._m21 + m._m12) * num2;
    quaternion.z = 0.5 * num5;
    quaternion.w = (m._m01 - m._m10) * num2;
    return quaternion;
}