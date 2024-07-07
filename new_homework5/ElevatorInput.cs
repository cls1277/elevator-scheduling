using System.Collections.Concurrent;

class ElevatorInput {
    private ConcurrentQueue<PersonRequest> requests;
    public ElevatorInput() {
        requests = new ConcurrentQueue<PersonRequest>();

        // PersonRequest request = new(4, 5, 1, 1, 9.6);
        // requests.Enqueue(request);

        // PersonRequest request = new(2, 9, 1, 1, 4.1);
        // requests.Enqueue(request);
        // request = new(1, 10, 2, 2, 7.5);
        // requests.Enqueue(request);

        // PersonRequest request = new(10, 6, 2, 1, 1.9);
        // requests.Enqueue(request);
        // request = new(3, 5, 1, 2, 6.6);
        // requests.Enqueue(request);

        PersonRequest request = new(2, 8, 1, 1, 2.7);
        requests.Enqueue(request);
        request = new(11, 3, 2, 2, 4.3);
        requests.Enqueue(request);
        request = new(3, 8, 3, 3, 7.4);
        requests.Enqueue(request);

        // PersonRequest request = new(2, 8, 1, 1, 2.7);
        // requests.Enqueue(request);
        // request = new(11, 3, 2, 1, 4.3);
        // requests.Enqueue(request);
        // request = new(5, 4, 4, 1, 5.0);
        // requests.Enqueue(request);
        // request = new(3, 8, 3, 1, 7.4);
        // requests.Enqueue(request);
    }
     public PersonRequest? nextPersonRequest() {
        if(requests.IsEmpty)
            return null;
        else {
            if(requests.TryDequeue(out PersonRequest? pr))
                return pr;
            return null;
        }
    }
}