using System.Collections.Concurrent;

class RequestQueue {
    private ConcurrentQueue<PersonRequest> requests;
    private bool stop;
    public RequestQueue() {
        requests = new ConcurrentQueue<PersonRequest>();
        stop = false;
    }
    public ConcurrentQueue<PersonRequest> getRequests() {
        return requests;
    }
    public void add(PersonRequest pr) {
        requests.Enqueue(pr);
    }
    public PersonRequest? getFirst() {
        if(requests.TryDequeue(out PersonRequest? request))
            return request;
        return null;
    }
    public bool IsEmpty() {
        return requests.IsEmpty;
    }
    public void setStop(bool _stop) {
        stop = _stop;
    }
    public bool getStop() {
        return stop;
    }
    public PersonRequest? hasEqualFrom(PersonRequest pr_o) {
        PersonRequest? foundRequest = null;
        var tempQueue = new ConcurrentQueue<PersonRequest>();
        while(requests.TryDequeue(out PersonRequest? pr)) {
            if(foundRequest == null && pr.getC() > 0 && pr.getFromFloor() == pr_o.getFromFloor()) {
                foundRequest = pr;
                continue;
            }
            tempQueue.Enqueue(pr);
        }
        while(tempQueue.TryDequeue(out PersonRequest? item)) {
            requests.Enqueue(item);
        }
        return foundRequest;
    }
    public PersonRequest? getCLessZero() {
        PersonRequest? foundRequest = null;
        var tempQueue = new ConcurrentQueue<PersonRequest>();
        while(requests.TryDequeue(out PersonRequest? pr)) {
            if(foundRequest == null && pr.getC() < 0) {
                foundRequest = pr;
                continue;
            }
            tempQueue.Enqueue(pr);
        }
        while(tempQueue.TryDequeue(out PersonRequest? item)) {
            requests.Enqueue(item);
        }
        return foundRequest;        
    }
    public void Remove(PersonRequest item) {
        var tempQueue = new Queue<PersonRequest>();
        while(requests.TryDequeue(out PersonRequest? currentItem)) {
            if(!currentItem.Equals(item)) {
                tempQueue.Enqueue(currentItem);
            }
        }
        while(tempQueue.Count > 0) {
            requests.Enqueue(tempQueue.Dequeue());
        }
    }
}