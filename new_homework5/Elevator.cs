class Elevator {
    public RequestQueue queue;
    private int curFloor, count, fullPerson, moveTime, direction;
    private PersonRequest? pr;
    public Elevator() {
        curFloor = 1;
        queue = new RequestQueue();
        fullPerson = 6;
        moveTime = 400;
        count = 0;
        direction = 1; // up
    }
    public void add(PersonRequest pr) { queue.add(pr); }
    public void setStop(bool _stop) { queue.setStop(_stop); }
    public int getMoveTime() { return moveTime; }
    public int getFullPerson() { return fullPerson; }
    public PersonRequest? getPr() { return pr; }
    public void run() {
        while(!(queue.IsEmpty() && queue.getStop())) {
            pr = queue.getFirst();
            if(pr != null && Homework5.schedule != null) {
                var eq = new PriorityQueue<PersonRequest, PersonRequestComparator>();
                Move(pr, pr.getFromFloor(), eq, 1); // in
                Open(pr);
                In(pr);
                count ++;
                eq.Enqueue(pr, new PersonRequestComparator());
                Close(pr);

                while(eq.Count != 0) {
                    PersonRequest? tmp = eq.Dequeue();
                    Move(tmp, tmp.getToFloor(), eq, -1); // out
                    Open(tmp);
                    Out(tmp);
                    Close(tmp);
                    count --;
                }
            }
        }
    }

    private void Arrive(PersonRequest pr, PriorityQueue<PersonRequest, PersonRequestComparator> eq, int inORout) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string arrive_string = "["+oTime.TotalSeconds+"]ARRIVE-"+curFloor;
        arrive_string += "-"+pr.getElevatorId();
        Console.WriteLine(arrive_string);
        // if(inORout == 1) { // in
            List<PersonRequest> tmps = queue.getEqualFromDirection(pr.getElevatorId(), curFloor, direction);
            if(tmps.Count > 0) Open(pr);
            for(int i=0; i<tmps.Count; i++) {
                PersonRequest tmp = tmps[i];
                In(tmp);
                eq.Enqueue(tmp, new PersonRequestComparator());
                count ++;
                if(count == fullPerson) {
                    for(int j=i+1; j<tmps.Count; j++) {
                        queue.add(tmps[j]);
                    }
                    break;
                }
            }
            if(tmps.Count > 0) Close(pr);
        // } else if(inORout == -1) { // out
            List<PersonRequest> eq2 = [], temp_eq2 = [];
            while(eq.Count > 0) {
                eq2.Add(eq.Peek());
                temp_eq2.Add(eq.Dequeue());
            }
            bool isOpen = false;
            foreach(PersonRequest item in eq2) {
                if(curFloor == item.getToFloor()) {
                    if(!isOpen) {
                        Open(pr);
                        isOpen = true;
                    }
                    Out(item);
                    temp_eq2.Remove(item);
                    count --;
                }
            }
            if(isOpen) Close(pr);
            foreach(PersonRequest request in temp_eq2) {
                eq.Enqueue(request, new PersonRequestComparator());
            }
        // }
    }
    private void Move(PersonRequest pr, int floor, PriorityQueue<PersonRequest, PersonRequestComparator> eq, int inORout) {
        if(floor < curFloor) {
            direction = -1;
            while(curFloor > floor) {
                curFloor --;
                Thread.Sleep(getMoveTime());
                Arrive(pr, eq, inORout);
            }
        } else {
            direction = 1;
            while(curFloor < floor) {
                curFloor ++;
                Thread.Sleep(getMoveTime());
                Arrive(pr, eq, inORout);
            }
        }
    }
    private void Out(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string out_string = "["+oTime.TotalSeconds+"]OUT-"+pr.getPersonId()+"-"+curFloor;
        out_string += "-"+pr.getElevatorId();
        Console.WriteLine(out_string);
    }
    private void In(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string in_string = "["+oTime.TotalSeconds+"]IN-"+pr.getPersonId()+"-"+curFloor;
        in_string += "-"+pr.getElevatorId();
        Console.WriteLine(in_string);
    }
    private void Open(PersonRequest pr) {
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string open_string = "["+oTime.TotalSeconds+"]OPEN-"+curFloor;
        open_string += "-"+pr.getElevatorId();
        Console.WriteLine(open_string);
    }
    private void Close(PersonRequest pr){
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string close_string = "["+oTime.TotalSeconds+"]CLOSE-"+curFloor;
        close_string += "-"+pr.getElevatorId();
        Console.WriteLine(close_string);   
    }

    private class PersonRequestComparator : IComparer<PersonRequest> {
        public int Compare(PersonRequest? o1, PersonRequest? o2) {
            if(o1==null || o2==null) return 0;
            return o1.getToFloor() - o2.getToFloor();
        }
    }
}