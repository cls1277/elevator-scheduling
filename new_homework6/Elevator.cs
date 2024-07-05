class Elevator {
    public RequestQueue queue;
    private int curFloor, direction, count, fullPerson, moveTime;
    private readonly int Id;
    private PersonRequest? pr;
    public Elevator(int id) {
        curFloor = 1;
        direction = 0;
        queue = new RequestQueue();
        fullPerson = 6;
        moveTime = 400;
        Id = id;
        count = 0;
    }
    public int getCount() { return count; }
    public int getId() { return Id; }
    public void add(PersonRequest pr) { queue.add(pr); }
    public PersonRequest? getFirst() { return queue.getFirst(); }
    public void setStop(bool _stop) { queue.setStop(_stop); }
    public bool getStop() { return queue.getStop(); }
    public int getDirection() { return direction; }
    public int getCurFloor() { return curFloor; }
    public void setCurFloor(int curFloor) { this.curFloor = curFloor; }
    public void setMoveTime(int moveTime) { this.moveTime = moveTime; }
    public int getMoveTime() { return moveTime; }
    public void setFullPerson(int fullPerson) { this.fullPerson = fullPerson; }
    public int getFullPerson() { return fullPerson; }
    public PersonRequest? getPr() { return pr; }
    public void run() {
        while(!(queue.IsEmpty() && queue.getStop())) {
            pr = queue.getFirst();
            if(pr != null && Homework6.schedule != null) {
                if(pr.getC() > 0) {
                    var eq = new PriorityQueue<PersonRequest, PersonRequestComparator>();
                    // Console.WriteLine("newpr - BEGIN "+pr.getFromFloor()+" "+pr.getToFloor()+" "+curFloor);
                    int flag2 = Move(pr, pr.getFromFloor());
                    if(flag2 == 3) {
                        PersonRequest? reset_pr = null;
                        foreach(PersonRequest request in queue.getRequests()) {
                            if(request.getC() <= 0 && request.getElevatorId() == getId()) {
                                reset_pr = request;
                                queue.Remove(request);
                                break;
                            }
                        }
                        Reset(reset_pr);
                        PersonRequest temp_pr = pr;
                        int elevator = Homework6.schedule.reveive(pr);
                        pr.setElevatorId(elevator);
                        Homework6.schedule.eleAdd(elevator, pr);
                        foreach(PersonRequest pr2 in queue.getRequests()) {
                            if(pr2.Equals(temp_pr)) continue;
                            elevator = Homework6.schedule.reveive(pr2);
                            pr2.setElevatorId(elevator);
                            Homework6.schedule.eleAdd(elevator, pr2);
                        }
                        continue;
                    }
                    Open(pr);
                    In(pr);
                    count ++;
                    eq.Enqueue(pr, new PersonRequestComparator());
                    ChangeDir(pr.getToFloor());
                    while(count+1 <= getFullPerson()) {
                        PersonRequest? tmp = queue.hasEqualFrom(pr);
                        if(tmp==null) break;
                        In(tmp);
                        eq.Enqueue(tmp, new PersonRequestComparator());
                        count ++;
                    }
                    Close(pr);
                    // Console.WriteLine("END");

                    while(eq.Count != 0) {
                        PersonRequest? tmp = eq.Dequeue();
                        int flag = Move(tmp, tmp.getToFloor());
                        if(flag == 0) {
                            // Console.WriteLine("flag==0 - BEGIN");
                            Open(tmp);
                            Out(tmp);
                            count --;
                            while(eq.Count!=0 && eq.Peek().getToFloor() == tmp.getToFloor()) {
                                Out(eq.Dequeue());
                                count --;
                            }
                            Close(tmp);
                            // Console.WriteLine("flag==0 - END");
                        } else if(flag == 3) {
                            // Console.WriteLine("flag==3 - BEGIN");
                            PersonRequest? reset_pr = null;
                            foreach(PersonRequest request in queue.getRequests()) {
                                if(request.getC() <= 0 && request.getElevatorId() == getId()) {
                                    reset_pr = request;
                                    queue.Remove(request);
                                    break;
                                }
                            }
                            Open(tmp);
                            Out(tmp);
                            Close(tmp);
                            int temp_curFloor = curFloor;
                            Reset(reset_pr);
                            PersonRequest neq2 = new(1, temp_curFloor, tmp.getToFloor(), tmp.getPersonId(), tmp.getTimeStamp());
                            int elevator = Homework6.schedule.reveive(neq2);
                            neq2.setElevatorId(elevator);
                            // Console.WriteLine("DEBUG"+elevator);
                            Homework6.schedule.eleAdd(elevator, neq2);
                            while(eq.Count != 0) {
                                PersonRequest neq = eq.Dequeue();
                                neq2 = new(1, temp_curFloor, neq.getToFloor(), neq.getPersonId(), neq.getTimeStamp());
                                // queue.add(neq2);
                                elevator = Homework6.schedule.reveive(neq2);
                                neq2.setElevatorId(elevator);
                                Homework6.schedule.eleAdd(elevator, neq2);
                            }
                            count = 0;
                            // Console.WriteLine("flag==3 - END");
                            break;
                        }
                    }
                } else {
                    Reset(pr);
                }
            }
        }
    }
    private void OpenOutClose(PersonRequest tmp) {
        if(Homework6.schedule == null) return ;
        Open(tmp);
        Out(tmp);
        int elevator = Homework6.schedule.reveive(tmp);
        PersonRequest npr = new(1, curFloor, tmp.getToFloor(), tmp.getPersonId(), 0);
        npr.setElevatorId(elevator);
        Homework6.schedule.eleAdd(elevator, npr);

        foreach(PersonRequest request in queue.getRequests()) {
            Out(request);
            // elevator = Homework6.schedule.reveive(request);
            npr = new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), request.getTimeStamp());
            elevator = Homework6.schedule.reveive(npr);
            npr.setElevatorId(elevator);
            Homework6.schedule.eleAdd(elevator, npr);
        }
        Close(tmp);
    }

    private void ChangeDir(int floor) {
        if(floor > curFloor) {
            direction = 1;
        } else if(floor < curFloor) {
            direction = -1;
        } else {
            direction = 0;
        }
    }

    private void Arrive(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string arrive_string = "["+oTime.TotalSeconds+"]ARRIVE-"+curFloor;
        arrive_string += "-"+pr.getElevatorId();
        Console.WriteLine(arrive_string);
    }

    private int Move(PersonRequest pr, int floor) {
        ChangeDir(floor);
        if(floor < curFloor) {
            while(curFloor > floor) {
                // 检查queue中是否有RESET
                foreach(PersonRequest request in queue.getRequests()) {
                    if(request.getC() <= 0 && request.getElevatorId() == getId()) {
                        return 3;
                    }
                }
                curFloor --;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
            }
        } else {
            while(curFloor < floor) {
                foreach(PersonRequest request in queue.getRequests()) {
                    if(request.getC() <= 0) {
                        return 3;
                    }
                }
                curFloor ++;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
            }
        }
        return 0;
    }
    private void Out(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string out_string = "["+oTime.TotalSeconds+"]OUT-"+pr.getPersonId()+"-"+curFloor;
        out_string += "-"+pr.getElevatorId();
        Console.WriteLine(out_string);
    }
    private void In(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string in_string = "["+oTime.TotalSeconds+"]IN-"+pr.getPersonId()+"-"+curFloor;
        in_string += "-"+pr.getElevatorId();
        Console.WriteLine(in_string);
    }
    private void Open(PersonRequest pr) {
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string open_string = "["+oTime.TotalSeconds+"]OPEN-"+curFloor;
        open_string += "-"+pr.getElevatorId();
        Console.WriteLine(open_string);
    }
    private void Close(PersonRequest pr){
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework6.beginTime);
        string close_string = "["+oTime.TotalSeconds+"]CLOSE-"+curFloor;
        close_string += "-"+pr.getElevatorId();
        Console.WriteLine(close_string);   
    }
    private void Reset(PersonRequest? pr) {
        if(pr == null) return ;
        // Console.WriteLine("RESET - BEGIN");
        TimeSpan oTime;
        oTime = DateTime.Now.Subtract(Homework6.beginTime);
        Console.WriteLine("["+oTime.TotalSeconds+"]RESET_ACCEPT-"+pr.getElevatorId()+"-"+pr.getFullPerson()+"-"+pr.getMoveTime());
        bool needOpen = false;
        foreach(PersonRequest request in queue.getRequests()) {
            if(request.getC() <= 0) continue;
            needOpen = true;
            break;
        }
        if(needOpen) {
            Open(pr);
            RequestQueue new_queue = new();
            foreach(PersonRequest request in queue.getRequests()) {
                if(request.getC() <= 0) continue;
                Out(request);
                new_queue.add(new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), 0));
            }
            queue = new_queue;
            Close(pr);
        }
        if(getId() <= 6) {
            oTime = DateTime.Now.Subtract(Homework6.beginTime);
            Console.WriteLine("["+oTime.TotalSeconds+"]RESET_BEGIN-"+pr.getElevatorId());
        }
        Thread.Sleep(1200);
        setMoveTime(pr.getMoveTime());
        setFullPerson(pr.getFullPerson());
        if(getId() <= 6) {
            oTime = DateTime.Now.Subtract(Homework6.beginTime);
            Console.WriteLine("["+oTime.TotalSeconds+"]RESET_END-"+pr.getElevatorId());
        }
        // Console.WriteLine("RESET - END");
    }
    private class PersonRequestComparator : IComparer<PersonRequest> {
        public int Compare(PersonRequest? o1, PersonRequest? o2) {
            if(o1==null || o2==null) return 0;
            return o1.getToFloor() - o2.getToFloor();
        }
    }
}