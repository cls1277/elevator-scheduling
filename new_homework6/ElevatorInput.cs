using System.Collections.Concurrent;

class ElevatorInput {
    private ConcurrentQueue<PersonRequest> requests;
    public ElevatorInput() {
        requests = new ConcurrentQueue<PersonRequest>();

        // PersonRequest request = new(0, 8, 300, 1, 4.6);
        // requests.Enqueue(request);
        // request = new(1, 5, 1, 2, 5.6);
        // requests.Enqueue(request);
        // request = new(0, 5, 400, 1, 12.6);
        // requests.Enqueue(request);

        // PersonRequest request = new(1, 2, 9, 1, 4.1);
        // requests.Enqueue(request);
        // request = new(0, 8, 300, 3, 4.6);
        // requests.Enqueue(request);
        // request = new(0, 8, 300, 2, 4.8);
        // requests.Enqueue(request);
        // request = new(0, 8, 300, 1, 7.5);
        // requests.Enqueue(request);
        // request = new(1, 1, 10, 2, 7.5);
        // requests.Enqueue(request);
        // request = new(1, 3, 9, 3, 10);
        // requests.Enqueue(request);

        PersonRequest request = new(0, 5, 400, 3, 1.1);
        requests.Enqueue(request);
        request = new(1, 10, 6, 2, 1.9);
        requests.Enqueue(request);
        request = new(0, 6, 300, 1, 5.1);
        requests.Enqueue(request);
        request = new(1, 3, 5, 1, 6.6);
        requests.Enqueue(request);
        request = new(0, 6, 400, 2, 8.3);
        requests.Enqueue(request);
        request = new(0, 6, 400, 5, 9.0);
        requests.Enqueue(request);
        request = new(0, 8, 300, 4, 10.0);
        requests.Enqueue(request);
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