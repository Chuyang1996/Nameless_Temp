Shader "Sbin/ff2" {
    // ��ͼ����
    properties{
        // ������("������",����)=ֵ
        _Color("����", color) = (1,1,1,1)
        _Ambient("������", color) = (0.3,0.3,0.3,0.3)
        _Specular("�߹�", color) = (1,1,1,1)

        // ������("������",range(������Сֵ,�������ֵ)=Ĭ��ֵ
        _Shininess("�߹�ǿ��",range(0,8)) = 4
        _Emission("�Է���", color) = (1,1,1,1)

        _Constant("͸��ͨ��", color) = (1,1,1,0.3)

        _MainTex("����", 2d) = ""
        _SecondTex("�ڶ�������",2d) = ""


    }

        SubShader{
    Tags { "Queue" = "Transparent" }


            pass {

                Blend SrcAlpha OneMinusSrcAlpha

                material {
                    diffuse[_Color]
                    ambient[_Ambient]
                    specular[_Specular]
                    shininess[_Shininess]
                    emission[_Emission]
                }
                lighting on // ���ù���
                separatespecular on  // ����߹�

                // ��������
                settexture[_MainTex] {
                    // �ϲ� ��ǰ���� * ǰ�����в��ʺ͹��յ���ɫ
                    // primary ��������պ����ɫ
                    // double ��ɫ*2
                    // quad ��ɫ*4
                    combine texture * primary double
                }

                    // �ڶ�������
                    settexture[_SecondTex] {
                    // �õ�ǰ���õ���������֮ǰ���в������Ľ�����л��        

                    //combine texture * previous double

                    // , �ź���Ĳ�������ֻ��ȡ������alphaͨ��, ǰ�����е���ɫalphaֵʧЧ
                    constantcolor[_Constant]
                    combine texture * previous double, texture * constant
                }
            }
    }
        // FallBack "Diffuse"
}