// classic camera (at the start and in main turnings)
public class GenericCamera : AbstractReplayCamera {
    
	void Start ()
    {
        range = 30;
    }

    // follow the car when it pass by
    protected override void UpdateCamera()
    {
        transform.LookAt(car.transform);
    }
}
