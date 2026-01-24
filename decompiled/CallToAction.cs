public class CallToAction : Wrapper
{
	public static CallToAction env;

	public Fader Fader;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
	}

	public void PlayExitSound()
	{
		speakers[0].TriggerSound(0);
	}
}
