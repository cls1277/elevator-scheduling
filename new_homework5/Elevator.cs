class Elevator {
    public RequestQueue queue;
    private int curFloor, count, fullPerson, moveTime;
    private PersonRequest? pr;
    public Elevator() {
        curFloor = 1;
        queue = new RequestQueue();
        fullPerson = 6;
        moveTime = 400;
        count = 0;
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
                Move(pr, pr.getFromFloor());
                Open(pr);
                In(pr);
                count ++;
                eq.Enqueue(pr, new PersonRequestComparator());
                while(count+1 <= getFullPerson()) {
                    PersonRequest? tmp = queue.hasEqualFrom(pr);
                    if(tmp==null) break;
                    In(tmp);
                    eq.Enqueue(tmp, new PersonRequestComparator());
                    count ++;
                }
                Close(pr);

                while(eq.Count != 0) {
                    PersonRequest? tmp = eq.Dequeue();
                    Move(tmp, tmp.getToFloor());
                    Open(tmp);
                    Out(tmp);
                    count --;
                    while(eq.Count!=0 && eq.Peek().getToFloor() == tmp.getToFloor()) {
                        Out(eq.Dequeue());
                        count --;
                    }
                    Close(tmp);
                    count = 0;
                    break;
                }
            }
        }
    }

    private void Arrive(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework5.beginTime);
        string arrive_string = "["+oTime.TotalSeconds+"]ARRIVE-"+curFloor;
        arrive_string += "-"+pr.getElevatorId();
        Console.WriteLine(arrive_string);
    }

    private void Move(PersonRequest pr, int floor) {
        if(floor < curFloor) {
            while(curFloor > floor) {
                curFloor --;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
            }
        } else {
            while(curFloor < floor) {
                curFloor ++;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
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