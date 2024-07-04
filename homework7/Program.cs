class Homework7 {
    public static DateTime beginTime;
    public static Schedule? schedule;
    public static Input? input;
    static void Main() {
        beginTime = DateTime.Now;
        RequestQueue queue = new();
        input = new Input(queue);
        Thread producer = new(input.run);

        schedule = new Schedule(queue);
        Thread consumer = new(schedule.run);
        
        producer.Start();
        consumer.Start();
    }
}