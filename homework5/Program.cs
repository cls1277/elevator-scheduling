using System;
using System.Threading;

class Homework5 {
    public static DateTime beginTime;
    static void Main() {
        beginTime = DateTime.Now;
        RequestQueue queue = new();
        Thread producer = new(new Input(queue).run);
        producer.Name = "input";

        Thread consumer = new(new Schedule(queue).run);
        consumer.Name = "schedule";
        
        producer.Start();
        consumer.Start();
    }
}