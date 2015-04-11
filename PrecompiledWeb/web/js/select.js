
		function  checkAll(allName,sName)
			{
				var i,length,vName;	
				var o=eval("document.forms[0]."+allName+".checked");
				length=document.forms[0].elements.length;
				for(i=0;i<length;i++)
				{
					vName=document.forms[0].elements[i].name;		
					if (vName==sName)
					{
						if (!document.forms[0].elements[i].disabled)
						{
							document.forms[0].elements[i].checked=o;
						}
					}
				}
			}
