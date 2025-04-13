using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class MyTests
{
    [Test]
    public void test1()
    {
        int f = 0;
        Debug.Log("Hello jack test");
        Assert.AreEqual(0, f);
    }

    [Test]
    public void testProjectileVelocity()
    {
        Vector3 startPosition = new Vector3(5f, 0.3f, 0f);
        Vector3 endPosition = PickupPolo.Constants.aiShootLocation;
        Vector3 v = PickupPolo.Queries.calculateProjectileVelocity(
            startPosition,
            endPosition,
            PickupPolo.Constants.shotSpeed,
            PickupPolo.Constants.ball_gravity.y
        );
        Assert.AreEqual(0, 0);
    }

    [Test]
    public void testProjectileAngle1()
    {
        float theta = PickupPolo.Queries.calculateProjectileAngle(
            deltaHorizontal: 10f,
            deltaY: -0.3f,
            initialSpeed: 15f,
            gravity: -10f
        );
        float thetaDegrees = Mathf.Rad2Deg*theta;
        Assert.AreEqual(11.37, thetaDegrees, 0.01f);
    }
    
    [Test]
    public void testProjectileAngle2()
    {
        float theta = PickupPolo.Queries.calculateProjectileAngle(
            deltaHorizontal: 100f,
            deltaY: -0.3f,
            initialSpeed: 15f,
            gravity: -10f
        );
        float thetaDegrees = Mathf.Rad2Deg*theta;
        Assert.That(float.IsNaN(thetaDegrees));
    }
    
    [Test]
    public void shootBallTest()
    {
        Vector3  v = PickupPolo.Queries.throwBallVelocity(
            startPosition: new Vector3(10f, 0.3f, 0f),
            endPosition: new Vector3(0f,0f,0f),
            initialSpeed: 15f,
            gravity: -10f,
            defaultAngle: Mathf.PI / 6
        );
        Debug.Log(v);
    }
}



