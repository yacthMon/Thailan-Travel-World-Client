using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingDemo : MonoBehaviour {
    public GameObject ponlatatNPC;
    public GameObject TodZaGunNPC;
    public GameObject SangAhTidNPC;
    public GameObject MaleeNPC;
    public AudioClip[] ponlatatNPCVoice;
    public AudioClip[] TodZaGunNPCVoice;
    public AudioClip[] SangAhTidNPCVoice;
    public AudioClip[] MaleeNPCCVoice;
    // Use this for initialization
    void Start () {
        NPCStatus ponlatatStatus = ponlatatNPC.GetComponent<NPCStatus>();
        NPCStatus todzagunStatus = TodZaGunNPC.GetComponent<NPCStatus>();
        NPCStatus sangahtidStatus = SangAhTidNPC.GetComponent<NPCStatus>();
        NPCStatus maleeStatus = MaleeNPC.GetComponent<NPCStatus>();
        //Ponlatat
        QuestCondition pcond = new QuestCondition(2, 10004 , 3); // ฆ่า สามล้อ 3 คัน
        Conversation q1openCon = new Conversation(new string[] { "รบกวนช่วยปราบสามล้อคลั่ง ที่อาละวาดอยู่นอกเมืองด้วยเถอะ", "ชาวเมืองเดือดร้อนกันมากเลย"},// Dialogue
            new AudioClip[] { ponlatatNPCVoice[0], ponlatatNPCVoice[1] }); // open Quest        
        Conversation q1inproCon = new Conversation(new string[] { "หากจะออกไปนอกเมืองละก็ มันค่อนข้างไกลนะ ลองขึ้นป้ายรถเมล์หน้าวัดดูซิ" },
            new AudioClip[] { ponlatatNPCVoice[2] });// in process Quest
        Conversation q1doneCon = new Conversation(new string[] { "ขอบคุณมากเลยโยม เจริญพร" },
            new AudioClip[] { ponlatatNPCVoice[3] });// done Quest
        Quest q1 = new Quest(1, "กำจัดสามล้อนอกเมือง", "กำจัดสามล้อที่อยู่นอกเมืองจำนวน 3 คัน", "Test Place Detail", null , pcond, 100, new Conversation[3] { q1openCon,q1inproCon,q1doneCon});
        q1.rewardEXP = 50;
        ponlatatStatus.questList.Add(q1);
        //Ponlatat


        // แสงอาทิตย์
        QuestCondition scond = new QuestCondition(1, 110, 0); // เดินไปหายักษ์ทศกัณฑ์
        Conversation q2openCon = new Conversation(new string[] { "ท่านช่วยข้ามแม่น้ำเจ้าพระยาที่อยู่ถัดจากที่นี่ ไปยังวัดอรุณเพื่อเจรจาบอกทศกัณฑ์ ยักษ์เขียวที่อยู่ที่นั่นให้หน่อยสิว่าตอนนี้ ข้ายังไม่พร้อมคืนเงินที่เคยติดไว้ ", "รบกวนช่วยข้าหน่อยนะ !" }); // open Quest // No sound
        Conversation q2inproCon = new Conversation(new string[] { "เจ้ายักษ์ทเขียวนั่นอยู่ที่วัดอรุณ ตรงข้ามแม่น้ำเจ้าะพระยาหน่ะ :3" });// in process Quest // No sound
        Conversation q2doneCon = new Conversation(new string[] { "อิอิ may be not use" });// done Quest
        Conversation q2recieveCon = new Conversation(new string[] {"หืม เจ้ากำลังตามหายักษ์เขียวอยู่งั้นหรือ", "ข้านี่แหละยักษ์เขียว ทศกัณฑ์" },
            new AudioClip[] { TodZaGunNPCVoice[0], TodZaGunNPCVoice[1] });// receiver Quest

        Quest q2 = new Quest(2, "การผ่อนผันหนี้ของพญาแสงอาทิตย์", "รบกวนเจ้าไปบอกผ่อนผันหนี้ของข้ากับเจ้ายักษ์ทศกัณฑ์ที่อยู่วัดแจ้งให้หน่อยสิ :3" , "Test Place Detail" , new Quest[1] { q1}, scond, sangahtidStatus.NPCID, 
            new Conversation[4] { q2openCon, q2inproCon, q2doneCon, q2recieveCon },todzagunStatus.NPCID);
        sangahtidStatus.questList.Add(q2);
        /*sangahtidStatus.conversation = new Conversation[1] { new Conversation(new string[2] { "สวัสดีท่านนักเดินทาง ข้าฝากไหว้วานท่านช่วยข้าสักเรื่องทีได้ไหม",
            "ท่านช่วยข้ามแม่น้ำเจ้าพระยาที่อยู่ถัดจากที่นี่ ไปยังวัดอรุณเพื่อเจรจาบอกทศกัณฑ์ ยักษ์เขียวที่อยู่ที่นั่นให้หน่อยสิว่าตอนนี้ ข้ายังไม่พร้อมคืนเงินที่เคยติดไว้ รบกวนช่วยข้าหน่อยนะ !" }) };*/
        // แสงอาทิตย์ */
        // ทศกัณฑ์
        QuestCondition tcond = new QuestCondition(3, 10001, 1); // ฆ่าทศกัณฑ์ (Boss id 10001) 1 ตัว
        Conversation q3openCon = new Conversation(new string[] { "ว่าไงนะ นี่เจ้ามาผ่อนผันเรื่องหนี้แทนอย่างนั้นหรือ ตกลงนี่เจ้าเป็นพวกของยักษ์วัดโพธิ์อย่างนั้นสินะ เห็นทีข้าคงปล่อยเจ้าไปไม่ได้แล้ว" },
            new AudioClip[] { TodZaGunNPCVoice[3] }); // open Quest
        Conversation q3inproCon = new Conversation(new string[] { "ต้องประลองกันสักตั้งแล้ว" });// in process Quest
        Conversation q3doneCon = new Conversation(new string[] { "ท่านแข็งแกร่งมากจริงๆ ท่านเป็นใครมาจากไหนหรือ", "อา…งั้นท่านก็ไม่ใช่พวกของยักษ์วัดโพธิ์สินะ ข้าต้องขออภัยจริงๆที่ไปหาเรื่องท่าน",
        "ที่จริงแล้วข้ากับพญาแสงอาทิตย์ ยักษ์ที่เฝ้าอยู่วัดโพธิ์นั้นเป็นเพื่อนรักกันมาก่อน วันหนึ่งยักษ์วัดโพธิ์ข้ามแม่น้ำเจ้าพระยามาขอยืมเงินข้า","โดยบอกว่าจะนำเงินมาส่งข้าครบทุกจำนวน แต่วันเวลาผ่านไปข้ากลับไม่ได้เงินนั้นเสียที",
        "แต่เรื่องหนี้เป็นเรื่องของพวกข้าสองตน ดังนั้นข้าคิดว่าข้าต้องไปจัดการปัญหานี้ด้วยตนเอง เห็นทีคงปล่อยเจ้ายักษ์นั่นไปไม่ได้เสียแล้ว!"},
            new AudioClip[] { TodZaGunNPCVoice[4], TodZaGunNPCVoice[5], TodZaGunNPCVoice[6] , TodZaGunNPCVoice[7], TodZaGunNPCVoice[8] });// done Quest
        q3doneCon.fade = true;
        Quest q3 = new Quest(3, "การเจรจากับยักษ์วัดแจ้ง", "ข้าเองแหละ ยักษ์ทศกัณฑ์" , "Test Place Detail" , new Quest[1] { q2 } /*null*/, tcond, todzagunStatus.NPCID,new Conversation[3] { q3openCon, q3inproCon, q3doneCon });
        todzagunStatus.questList.Add(q3);
        /*todzagunStatus.conversation = new Conversation[1] { new Conversation(new string[] { "ท่านมีธุระอะไรกับข้าหรือ",
            "ว่าไงนะ นี่เจ้ามาผ่อนผันเรื่องหนี้แทนอย่างนั้นหรือ ตกลงนี่เจ้าเป็นพวกของยักษ์วัดโพธิ์อย่างนั้นสินะ เห็นทีข้าคงปล่อยเจ้าไปไม่ได้แล้ว" }) };*/
        // ทศกัณฑ์ */
        // มาลี
        QuestCondition mcond = new QuestCondition(1, 110, 0);
        Conversation q4openCon = new Conversation(new string[] { "ขอบคุณมากเลยค่ะ ยักษ์วัดแจ้งกับยักษ์วัดโพธิ์มีพละกำลังมหาศาลมากถ้าคุณพี่ไม่ช่วย ต้นไม้ใบหญ้าบริเวณนั้นต้องราบเรียบเตียนหมดแน่ๆ",
            "โอม นะมัส ศิวายะ นี่เป็นบทสวดบูชาพระอิศวร คุณพี่ช่วยท่องบทสวดนี้เพื่ออัญเชิญให้พระอิศวร ช่วยมาห้ามปรามยักษ์ทั้งสองตนทีได้ไหม", "พระอิศวรท่านมีอำนาจมาก ท่านต้องช่วยไม่ให้บ้านเมืองเราโดนทำลายแน่นอนค่ะ ช่วยทีนะคะ" }
            ); // open Quest // No sound
        Conversation q4inproCon = new Conversation(new string[] { "เร็วเข้า ยักษ์ทั้ง 2 ตนกำลังสู้อยู่ที่ท่าเตียน ฝั่งตรงข้ามแม่น้ำเจ้าพระยา.." });// in process Quest // No sound
        Conversation q4doneCon = new Conversation(new string[] { "not use malee 0" });// done Quest
        Conversation q4recieveCon = new Conversation(new string[] { "โอม นะมัส ศิวายะ","อา… ไม่นะ นั่นพระอิศวรนี่ ได้โปรดอย่าทำอะไรข้าเลย", "ได้โปรดท่านผู้เมตตา อย่าสาปข้าให้เป็นหินเลย ข้าผิดไปแล้ว อ๊ากกกกก.." },
            new AudioClip[] { null,TodZaGunNPCVoice[9],SangAhTidNPCVoice[0]});// receiver Quest
        q4recieveCon.fade = true;
        Quest q4 = new Quest(4, "ช่วยบ้านเมืองเราที", "มาลีไหว้วานให้เราห้ามศึกครั้งนี้ โดยที่ให้ไปค้นหาบทอัญเชิญพระศิวะผู้มีพลังในการสาปยักษ์ทั้งคู่ให้เป็นหินได้ โดยบทสวดสามารถหาได้จากมอนสเตอร์ในบริเวณใกล้เคียง เมื่อหาได้ให้ไปร่ายบทอัญเชิญกับยักษ์ จะมีออร่าเหมือนพระศิวะปรากฏขึ้นมาแล้วจะสามารถสาปยักษ์ให้เป็นหินได้"
            , "Test Place Detail" , new Quest[1] { q3 }, mcond, maleeStatus.NPCID, new Conversation[4] { q4openCon, q4inproCon, q4doneCon, q4recieveCon }, todzagunStatus.NPCID);


        QuestCondition mcond2 = new QuestCondition(0, maleeStatus.NPCID,0); // รายงานหนูมาลี
        Conversation q5openCon = new Conversation(new string[] { "not use malee 1" }); // open Quest
        Conversation q5inproCon = new Conversation(new string[] { "not use malee 2" });// in process Quest
        Conversation q5doneCon = new Conversation(new string[] { "ขอบคุณคุณพี่มากๆเลยนะคะ ที่ช่วยกอบกู้บ้านเมืองของเราไว้ได้ ถึงแม้ว่าพื้นที่แห่งนั้นจะถูกทำลายจนราบเรียบเป็นหน้ากลองก็ตามที",
        "จากนี้ที่นี่คงเป็นตำนานการต่อสู้กันของยักษ์ทั้งสองวัดที่โดน สาปเป็นหินด้วยอำนาจของพระอิศวร ซึ่งชาวบ้านแถวนี้ต่างพากันเรียกสถานที่แห่งนั้นว่า ท่าเตียน ", "แต่หนูมาลีคิดว่าที่นี่จะเป็นตำนานให้คนรุ่นหลังมา ศึกษาต่อแน่นอนค่ะต่อจากนี้พวกเราจะช่วยกัน ดูแลรักษาบ้านเมืองให้น่าอยู่ขอบคุณคุณพี่มากนะคะ "},
        new AudioClip[] { MaleeNPCCVoice[0], MaleeNPCCVoice[1], MaleeNPCCVoice[2] });// done Quest
        Quest q5 = new Quest(5, "กลับไปหาหนูมาลี", "ยักษ์ยุติการต่อสู้กันแล้ว ต้องรีบไปบอกหนูมาลี" , "Test Place Detail" , null , mcond2, maleeStatus.NPCID, new Conversation[3] { q5openCon, q5inproCon, q5doneCon });
        q4.questContinue = q5; // เพิ่มเควสต่อเนื่อง
        maleeStatus.questList.Add(q4);
        // มาลี */

        Debug.Log("Finished setting demo");
        GameObject.Find("NPC_System").BroadcastMessage("refreshUI"); // broadcast message to all child of NPC_System
    }

    public GameObject todzagun;
    public GameObject sangahtid;
    public void AfraidGiant() {
        todzagun.GetComponent<Animator>().Play("afraid");
        sangahtid.GetComponent<Animator>().Play("afraid");
    }
}