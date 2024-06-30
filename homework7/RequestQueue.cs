class RequestQueue {
    private List<PersonRequest> requests;
    private bool stop;
    public RequestQueue() {
        requests = [];
        stop = false;
    }
    public List<PersonRequest> getRequests() {
        return requests;
    }
    public void add(PersonRequest pr) {
        requests.Add(pr);
    }
    public PersonRequest? getFirst() {
        if(requests.Count > 0) {
            PersonRequest pr = requests[0];
            requests.RemoveAt(0);
            return pr;
        } else {
            return null;
        }
    }
    public void setStop(bool _stop) {
        stop = _stop;
    }
    public bool getStop() {
        return stop;
    }
    public bool IsEmpty() {
        if(requests.Count == 0) {
            return true;
        } else {
            return false;
        }
    }
    public PersonRequest? hasEqualFrom(PersonRequest pr_o) {
        foreach(PersonRequest pr in requests) {
            if(pr.getC()>0 && pr.getFromFloor() == pr_o.getFromFloor()) {
                requests.Remove(pr);
                return pr;
            }
        }
        return null;
    }
}