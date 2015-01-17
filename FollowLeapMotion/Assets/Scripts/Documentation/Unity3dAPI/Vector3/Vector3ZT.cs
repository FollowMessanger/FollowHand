using UnityEngine;
using System.Collections;

public class Vector3ZT : MonoBehaviour {

    public Transform robot;
    public Transform target;
    public Transform distance;
    public Transform dot;
    public Transform startPosition;
    public Transform endPosition;
    public Transform moveTowards;
    public Transform project;
    public Transform originalObject;
    public Transform reflectedObject;
    public Transform rotateTowards;
    public float startTime,giornataLunga;
    public Vector3 centerPoint;
    public float raggio,speed;


	void Start () {

        // Vector3.Lerp
        startTime = Time.time;
        giornataLunga = Vector3.Distance(startPosition.position,endPosition.position);

	
	}
	
	void Update () {

        // INDIVIDUAZIONE MATEMATICA DEGLI OGGETTI SENZA IL BISOGNO DI COLLIDER O ALTRO
        //*****************************************************************************
        // Vector3.sqrMagnitude

        if (robot)
        {
            Vector3 robotVector = robot.position - transform.position;
            float sqrLenght = robotVector.sqrMagnitude;
            if(sqrLenght < 25f)
            {
                Debug.Log("OBJECT RELEVED");
            }

        }

        //*****************************************************************************
        // Vector3.magnitude
        // fa ritornare la lunghezza del vettore 
        // la formula � radice quadrata di x*x + y*y + z*z
        // se non si usa la funzione Vecto3.magnitude il ci� si traduce cosi -> Mathf.Sqrt(Vector3.Dot(v, v)); 
        // <- v � un singolo vettore , nel dot si scrive 2 volte visto che si fa la moltiplicazione di tutti gli 
        // assi moltiplicati per il rispettivo coseno
        Vector3 vector = new Vector3(0, 0, 0);
       // Vector3 vector = new Vector3(0, 0, Time.deltaTime);
        transform.Translate(vector);
        Debug.Log("Vector3.magnitude-> "+vector.magnitude);

        //*****************************************************************************
        // Vector3.normalized
        // ritorna un valore pari a 1 se il vettore va nella direzione x a 0.4 , se il valore � negativo
        // ritorner� 0 , quando stampa si visualizza (1.0,0.0,0.0)
        // ATTENZIONE alla differenza con Vector3.Normalize
        Vector3 normal = new Vector3(0.4f,0,0);
        Debug.Log("Vector3.Normalized-> "+normal.normalized);

        //******************************************************************************
        // Vector3.this[int]
        // esiste un altro modo di accedere alle variabili x,y,z di Vector3 ed � pensata come un array
        // [0] = x [1] = y [2] = z
        Vector3 alternative = new Vector3(0,0,0); ;
        alternative[0] = 1;

        //*******************************************************************************
        // Vector3.x
        // ovviamente si pu� accedere direttamente ai 3 assi..
        Vector3 assetX = new Vector3();
        assetX.x = 1;

        //*******************************************************************************
        // Vector3.Set
        // vuoi cambiare i valori di un Vector3 esistente?

        assetX.Set(0,1,1);

        //*******************************************************************************
        // Vector3.ToString()
        // si pu� stampare il valore attuale del vettore ma la stampa da informazioni uguale s Vector3.normalize
        Debug.Log("String-> "+assetX.ToString());

        //*******************************************************************************
        // FUNZIONI STATICHE
        //*******************************************************************************

        //*******************************************************************************
        // Vector3.Angle
        // ritorna l'angolo tra 2 vettori (between)
        // posso sapere grazie a un algoritmo utilizzante un angolo acuto la posizione in gradi
        // rispetto a un oggetto di un player in circolazione o viceversa
        
        Vector3 actualTarget  = target.position - transform.position;
        Vector3 forwardTarget = transform.forward;

        float angolo = Vector3.Angle(actualTarget,forwardTarget);
        Debug.Log("Angolo ->"+angolo);
        if (angolo < 5.0f)
        {
            Debug.Log("CLOSE");
        }

        //*******************************************************************************
        // Vector3.ClampMagnitude
        // restituisce una copia del vettore con il suo relativo magnitude (ipotenusa) ancorato al raggio
        // perci� l'oggetto si potr�  muovere solamente entro i limiti del raggio e non oltre

        Vector3 movimento   = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        Vector3 newPosition = transform.position + movimento;
        Vector3 copyVector = newPosition - centerPoint;

        transform.position = centerPoint + Vector3.ClampMagnitude(copyVector,raggio);

        //*******************************************************************************
        // Vector3.Cross
        // � il prodotto tra 2 vettori , come spiegava dell'oro nella lezione di fisica.. "il vettore risultante"
        // essi prima vengono moltiplicati tra di loro e dopo rimoltiplicati per il seno del angolo in ingresso
	    // REGOLA DELLA MANO
        // ...ma ATTENZIONE perch� il vettore risultante non � un vettore sullo stesso asse di quelli moltiplicati (non sempre)
        // perci� � molto probabile che il vettore risultante sia sull'altro asse 

        Vector3 vectorUP = new Vector3(0,1,0);
        Vector3 vectorFORWARD = new Vector3(0,0,1);
        Debug.Log("Cross-> "+Vector3.Cross(vectorUP,vectorFORWARD).normalized);

      /* 
        Debug.Log("CrossX-> " + Vector3.Cross(vectorUP, vectorFORWARD).x);
        Debug.Log("CrossY-> " + Vector3.Cross(vectorUP, vectorFORWARD).y);
        Debug.Log("CrossZ-> " + Vector3.Cross(vectorUP, vectorFORWARD).z);
      */
        
        Vector3 getNormalVector = GetNormalVector(movimento,vectorUP,vectorFORWARD);
        Debug.Log("FunctionVector3 -> "+ getNormalVector);

        //*******************************************************************************
        // Vector3.Distance
        // faccio ritornare in una variabile float la distanza tra un vettore a e un vettore b
        // se non voglio utilizzare questa funzione Vector3.Distance(a,b) sarebbe lo stesso 
        // scrivere (a-b).magnitude
        // la distanza non � mai negativa al massimo 0 ed � utile sapere quanto � distante un tale oggetto da me 

        if(distance) 
        {
            float distanceVector = Vector3.Distance(distance.position,transform.position);
            Debug.Log("Distance-> "+ distanceVector);
        }

        //*******************************************************************************
        // Vector3.Dot
        // i due vettori vengono moltiplicati tra di loro e rimoltiplicati per il coseno dell'angolo in ingresso
        // il valore risultante normalizzato � 1 = stessa direzione , -1 = direzioni opposte , 0 = direzioni perpendicolari
        // il tutto riguarda i vettori perci� le direzioni dei VETTORI sono perpendicolari 

        if(dot)
        {
            Vector3 forwardDot = transform.TransformDirection(Vector3.forward);
            // Vector3.forward = (0,0,1);
            Vector3 otherDot = dot.position - transform.position;
            if(Vector3.Dot(forwardDot,otherDot) < 0)
            {
                Debug.Log("DIREZIONI OPPOSTE");
            }
            if (Vector3.Dot(forwardDot, otherDot) > 0)
            {
                Debug.Log("STESSA DIREZIONE");
            }
            if (Vector3.Dot(forwardDot, otherDot) == 0)
            {
                Debug.Log("DIREZIONI PERPENDICOLARI");
            }

        }

        //*******************************************************************************
        // Vector3.Lerp
        // static Vector3 Lerp(Vector3 from, Vector3 to, float t);
        // questa funzione serve a far andare dolcemente un oggetto senza scatti perci� va bene usarla per spostare 
        // oggetti nel modo pi� naturale possibile

        float distanzaTrovata = (Time.time - startTime) * 1.0f;
        float metaGiornata = distanzaTrovata / giornataLunga;

        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, metaGiornata);

        //*******************************************************************************
        // Vector3.Max
        // questa funzione statica trova il valore massimo di ogni asse 
        // perci� nel confronto tra i 2 vettori il valore massimo non � quello del vettore A
        // ma � il valore massimo di ogni asse e cio� per le x = 3 , per le y = 4 e per le z = 7 

        Vector3 vectorA = new Vector3(1,4,7);
        Vector3 vectorB = new Vector3(3,1,3);

        Debug.Log("Max-> "+Vector3.Max(vectorA,vectorB));

        //*******************************************************************************
        // Vector3.Min
        // questa funzione statica trova il valore minimo di ogni asse 
        // perci� nel confronto tra i 2 vettori il valore minimo non � quello del vettore B
        // ma � il valore minimo di ogni asse e cio� per le x = 1 , per le y = 1 e per le z = 3 

        Vector3 vectorAMin = new Vector3(1, 4, 7);
        Vector3 vectorBMin = new Vector3(3, 1, 3);

        Debug.Log("Min-> " + Vector3.Min(vectorAMin, vectorBMin));

        //*******************************************************************************
        // Vector3.MoveTowards
        // questa funzione fa si che un dato oggetto di avvicini o insegui un altro oggetto a una certa velocit�
        // una sorta di calamita , simple ENEMY

        float steps = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveTowards.position, steps);

        Debug.Log("MoveTowards -> " + Vector3.MoveTowards(transform.position, moveTowards.position, steps));

        //*******************************************************************************
        // Vector3.Normalize
        // quando un vettore viene Normalizzato (ATTENZIONE che non � come Vector3.normalized) esso mantiene
        // la stessa direzione ma la sua lunghezza � 1.0 , in pratica questa funzione accorcia il vettore in 
        // base al fatto se lo fa variare a 1 se � troppo lungo o a 0 se esso � troppo piccolo 
        // il Vector3.normalized invece non riduce le dimensioni del vettore ma soltanto la sua direzione

        //*******************************************************************************
        // Vector3.OrthoNormalize
        // rende i vettori normalizzati (Vector3.Normalize) e ortogonali tra di loro cio� l'angolo tra di loro � di 90 gradi
        // si pu� intendere che 3 vettori normalizzati (lunghezza = 1) e ortogonali tra di loro rappresentino i 3 assi
        // del gioco infatti con questa funzione � possibile crearsi i propri assi cartesiani .
        // questa funzione da la possibilit� di modificare a piacere le forme dei solidi , se io voglio un exagono fatto
        // direttamente con unity LO POSSO FARE !!! (SIII:::PUOO:::::FAREEEEE:::!!!)
        // per l'esempio guarda lo script

        //*******************************************************************************
        // Vector3.Project
        // proietta un VETTORE su un altro VETTORE , se ho un vettore lungo 1 e magari ho un laser che deve arrivare
        // fino alla fine della stanza Vector3.Project far� si che un altro vettore sia sovvraposto al vettore standard
        // per arrivare a lunghezze pi� lunghe , attenzione che il vettore base non scompare ma nello stesso spazio ho 2 vettori
        // static Vector3 Project(Vector3 vector, Vector3 onNormal);
        //     <      ^
        //       \    ! projection
        //        \   !
        //  vector \  ^
        //          \ |  onNormal
        //           \|

        SlideProject(project,Vector3.forward);

        //*******************************************************************************
        // Vector3.Reflect
        // la funzione fa proprio quello che dice , fa riflettere un vettore su qualcosa
        // nell'esempio si riflette la posizione e cio significa creare una sorta di specchio
        // riflettendo la posizione di un cubo si avr� un cubo nella stessa posizione riflessa dal vettore

        reflectedObject.position = Vector3.Reflect(originalObject.position,Vector3.right);
        Debug.DrawLine(originalObject.position, Vector3.right,Color.green);

        //*******************************************************************************
        // Vector3.RotateTowards
        // static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta);
        // lavora come MoveTowards , cio� si sposta verso un determinato punto , una sorta di ENEMY , la differenza � che
        // il MoveThowards si sposta nella direzione voluta come posizione , invece il RotateTowards si sposta come rotazione
        // cio� si viene guardati per cosi dire , � come avere una telecamera che si sposta in base a dove ci spostiamo noi


        Vector3 targetTowards = rotateTowards.position - transform.position;
        float step = speed * Time.deltaTime;
        Vector3 newRotateTowards = Vector3.RotateTowards(transform.forward, targetTowards, step, 0.0f);
        Debug.DrawRay(transform.position, newRotateTowards , Color.red);
        transform.rotation = Quaternion.LookRotation(newRotateTowards);

        //*******************************************************************************
        // Vector3.Scale
        // static Vector3 Scale(Vector3 a, Vector3 b);
        // il risultato � una semplice moltiplicazione tra i due vettori

        Debug.Log("Scale-> "+Vector3.Scale(new Vector3(1, 2, 3), new Vector3(2, 3, 4)));

        //*******************************************************************************
        // Vector3.Slerp
        // � il contrario di Lerp se esso sposta gli oggetti Slerp li unisce , ad esempio fa entrare qualcosa dentro 
        // qualcos altro spostandolo
        // vedi esempio script

        //*******************************************************************************
        // Vector3.SmoothDamp
        // inserendo lo script nella telecamera spostandosi essa girer� intorno a un oggetto target 
        // vedi esempio script

    }

    public void SlideProject(Transform target, Vector3 direction)
    {
        Vector3 location = target.position - transform.position;
        Vector3 force = Vector3.Project(location,direction);
        rigidbody.AddForce(force);
    }

    public Vector3 GetNormalVector(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 lato1 = b - a;
        Vector3 lato2 = c - a;

        return Vector3.Cross(lato1, lato2).normalized;
    }

}
