class Elevator {
    public RequestQueue queue;
    private int curFloor, direction, count, fullPerson, moveTime, low, high;
    private readonly int Id;
    private bool single;
    private PersonRequest? pr;
    public Elevator(int id) {
        curFloor = 1;
        direction = 0;
        queue = new RequestQueue();
        fullPerson = 6;
        moveTime = 400;
        low = 1;
        high = 11;
        single = true;
        Id = id;
    }
    public int getId() {
        return Id;
    }
    public bool getSingle() {
        return single;
    }
    public void setSingle(bool single) {
        this.single = single;
    }

    public int getLow() {
        return low;
    }
    public void setLow(int low) {
        this.low = low;
    }
    public int getHigh() {
        return high;
    }
    public void setHigh(int high) {
        this.high = high;
    }

    public void add(PersonRequest pr) {
        queue.add(pr);
    }
    public PersonRequest? getFirst() {
        return queue.getFirst();
    }
    public void setStop(bool _stop) {
        queue.setStop(_stop);
    }
    public bool getStop() {
        return queue.getStop();
    }
    public int getDirection() {
        return direction;
    }
    public int getCurFloor() {
        return curFloor;
    }
    public void setCurFloor(int curFloor) {
        this.curFloor = curFloor;
    }
    public void setMoveTime(int moveTime) {
        this.moveTime = moveTime;
    }
    public int getMoveTime() {
        return moveTime;
    }
    public void setFullPerson(int fullPerson) {
        this.fullPerson = fullPerson;
    }
    public int getFullPerson() {
        return fullPerson;
    }
    public PersonRequest? getPr() {
        return pr;
    }
    public void run() {
        while (!(queue.IsEmpty() && queue.getStop())) {
            pr = queue.getFirst();
            if (pr != null && Homework7.schedule != null) {
                if(pr.getC() > 0) {
                    var eq = new PriorityQueue<PersonRequest, PersonRequestComparator>();
                    Move(pr, pr.getFromFloor());
                    Open(pr);
                    In(pr);
                    count ++;
                    eq.Enqueue(pr, new PersonRequestComparator());
                    ChangeDir(pr.getToFloor());
                    while (count+1 <= getFullPerson()) {
                        PersonRequest? tmp = queue.hasEqualFrom(pr);
                        if(tmp==null) break;
                        In(tmp);
                        eq.Enqueue(tmp, new PersonRequestComparator());
                        count ++;
                    }
                    Close(pr);

                    while (eq.Count != 0) {
                        PersonRequest? tmp = eq.Dequeue();
                        int flag = Move(tmp, tmp.getToFloor());
                        if(flag == 0) {
                            Open(tmp);
                            Out(tmp);
                            count --;
                            while (eq.Count!=0 && eq.Peek().getToFloor() == tmp.getToFloor()) {
                                Out(eq.Dequeue());
                                count --;
                            }
                            Close(tmp);
                        } else if(flag == 1) {
                            Open(tmp);
                            Out(tmp);
                            int elevator = Homework7.schedule.reveive(tmp);
                            PersonRequest npr = new PersonRequest(1, curFloor, tmp.getToFloor(), tmp.getPersonId(), -1, 0);
                            npr.setElevatorId(elevator);
                            Homework7.schedule.eleAdd(elevator, npr);

                            foreach(PersonRequest request in queue.getRequests()) {
                                Out(request);
                                elevator = Homework7.schedule.reveive(pr);
                                npr = new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), -1, 0);
                                npr.setElevatorId(elevator);
                                Homework7.schedule.eleAdd(elevator, npr);
                            }
                            Close(tmp);
                            Move(tmp, getLow()+1);
                        } else if(flag == 2) {
                            Open(tmp);
                            Out(tmp);
                            int elevator = Homework7.schedule.reveive(tmp);
                            PersonRequest npr = new PersonRequest(1, curFloor, tmp.getToFloor(), tmp.getPersonId(), -1, 0);
                            npr.setElevatorId(elevator);
                            Homework7.schedule.eleAdd(elevator, npr);

                            foreach(PersonRequest request in queue.getRequests()) {
                                Out(request);
                                elevator = Homework7.schedule.reveive(tmp);
                                npr = new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), -1, request.getTimeStamp());
                                npr.setElevatorId(elevator);
                                Homework7.schedule.eleAdd(elevator, npr);
                            }
                            Close(tmp);
                            Move(tmp, getHigh()-1);
                        }
                    }
                } else {
                    Reset(pr);
                }
            }
        }
    }

    public void ChangeDir(int floor) {
        if (floor > curFloor) {
            direction = 1;
        } else if (floor < curFloor) {
            direction = -1;
        } else {
            direction = 0;
        }
    }

    public void Arrive(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework7.beginTime);
        string arrive_string = "["+oTime.TotalSeconds+"]ARRIVE-"+curFloor;
        if(getSingle() == false) {
            arrive_string += "-";
            if(getId() <= 6) {
                arrive_string += pr.getElevatorId()+"-A";
            } else {
                arrive_string += pr.getElevatorId()-6;
                arrive_string += "-B";
            }
        } else {
            arrive_string += "-"+pr.getElevatorId();
        }
        Console.WriteLine(arrive_string);
    }

    public int Move(PersonRequest pr, int floor) {
        ChangeDir(floor);
        if(floor < curFloor) {
            while(curFloor > floor) {
                curFloor --;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
                if(curFloor == getLow() && curFloor > 1)
                    return 1;
            }
        } else {
            while(curFloor < floor) {
                curFloor ++;
                Thread.Sleep(getMoveTime());
                Arrive(pr);
                if(curFloor == getHigh() && curFloor < 11)
                    return 2;
            }
        }
        return 0;
    }
    public void Out(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework7.beginTime);
        string out_string = "["+oTime.TotalSeconds+"]OUT-"+pr.getPersonId()+"-"+curFloor;
        if(getSingle() == false) {
            out_string += "-";
            if(getId() <= 6) {
                out_string += pr.getElevatorId()+"-A";
            } else {
                out_string += pr.getElevatorId()-6;
                out_string += "-B";
            }
        } else {
            out_string += "-"+pr.getElevatorId();
        }
        Console.WriteLine(out_string);
    }
    public void In(PersonRequest pr) {
        TimeSpan oTime = DateTime.Now.Subtract(Homework7.beginTime);
        string in_string = "["+oTime.TotalSeconds+"]IN-"+pr.getPersonId()+"-"+curFloor;
        if(getSingle() == false) {
            in_string += "-";
            if(getId() <= 6) {
                in_string += pr.getElevatorId()+"-A";
            } else {
                in_string += pr.getElevatorId()-6;
                in_string += "-B";
            }
        } else {
            in_string += "-"+pr.getElevatorId();
        }
        Console.WriteLine(in_string);
    }
    public void Open(PersonRequest pr) {
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework7.beginTime);
        string open_string = "["+oTime.TotalSeconds+"]OPEN-"+curFloor;
        if(getSingle() == false) {
            open_string += "-";
            if(getId() <= 6) {
                open_string += pr.getElevatorId()+"-A";
            } else {
                open_string += pr.getElevatorId()-6;
                open_string += "-B";
            }
        } else {
            open_string += "-"+pr.getElevatorId();
        }
        Console.WriteLine(open_string);
    }
    public void Close(PersonRequest pr){
        Thread.Sleep(200);
        TimeSpan oTime = DateTime.Now.Subtract(Homework7.beginTime);
        string close_string = "["+oTime.TotalSeconds+"]CLOSE-"+curFloor;
        if(getSingle() == false) {
            close_string += "-";
            if(getId() <= 6) {
                close_string += pr.getElevatorId()+"-A";
            } else {
                close_string += pr.getElevatorId()-6;
                close_string += "-B";
            }
        } else {
            close_string += "-"+pr.getElevatorId();
        }
        Console.WriteLine(close_string);   
    }
    public void Reset(PersonRequest pr) {
        TimeSpan oTime;
        if(pr.getD() < 0) {
            oTime = DateTime.Now.Subtract(Homework7.beginTime);
            Console.WriteLine("["+oTime.TotalSeconds+"]RESET_ACCEPT-"+pr.getElevatorId()+"-"+pr.getFullPerson()+"-"+pr.getMoveTime());
        } else {
            if(getId() < 6) {
                oTime = DateTime.Now.Subtract(Homework7.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]RESET_ACCEPT-"+pr.getElevatorId()+"-"+pr.getD()+"-"+pr.getFullPerson()+"-"+pr.getMoveTime());
                setCurFloor(pr.getD()-1);
                setHigh(pr.getD());
                setSingle(false);
            } else {
                setCurFloor(pr.getD()+1);
                setLow(pr.getD());
                setSingle(false);
            }
        }
        if(queue.getRequests().Count > 0) {
            Open(pr);
            RequestQueue new_queue = new();
            foreach(PersonRequest request in queue.getRequests()) {
                oTime = DateTime.Now.Subtract(Homework7.beginTime);
                Console.WriteLine("["+oTime.TotalSeconds+"]OUT-"+request.getPersonId()+"-"+curFloor+"-"+request.getElevatorId());
                new_queue.add(new PersonRequest(1, curFloor, request.getToFloor(), request.getPersonId(), -1, 0));
            }
            queue = new_queue;
            Close(pr);
        }
        if(getId() < 6) {
            oTime = DateTime.Now.Subtract(Homework7.beginTime);
            Console.WriteLine("["+oTime.TotalSeconds+"]RESET_BEGIN-"+pr.getElevatorId());
        }
        Thread.Sleep(1200);
        setMoveTime(pr.getMoveTime());
        setFullPerson(pr.getFullPerson());
        if(getId() < 6) {
            oTime = DateTime.Now.Subtract(Homework7.beginTime);
            Console.WriteLine("["+oTime.TotalSeconds+"]RESET_END-"+pr.getElevatorId());
        }
    }
    private class PersonRequestComparator : IComparer<PersonRequest> {
        public int Compare(PersonRequest? o1, PersonRequest? o2) {
            if(o1==null || o2==null) return 0;
            return o1.getToFloor() - o2.getToFloor();
        }
    }
}