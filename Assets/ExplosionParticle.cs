using UnityEngine;

public class ExplosionParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sparkParticle;
    [SerializeField] private ParticleSystem _flameParticle;
    [SerializeField] private float _explosionForce;

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Keypad1))
            PlayExplosion();
	}

	public void PlayExplosion()
    {
		_sparkParticle.Play();
        _flameParticle.Play();
    }

	private void OnTriggerStay(Collider other)
	{
        if (!_sparkParticle.isPlaying)
            return;
        if (other.isTrigger)
            return;

        Rigidbody colliderBody = other.GetComponent<Rigidbody>();

        if (colliderBody == null)
            colliderBody.transform.parent.GetComponent<Rigidbody>();
        if (colliderBody == null)
            colliderBody.GetComponentInChildren<Rigidbody>();
        if (colliderBody == null)
            return;

        colliderBody.AddExplosionForce(_explosionForce, transform.position, 3);
	}
}
