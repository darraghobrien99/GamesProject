﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

interface Icontrollable
{

    void youve_been_touched();

    void MoveTo(Vector3 destination);

    void deselect_object();

    void scaleObject(Vector3 initialScale, float factor);



  

}
