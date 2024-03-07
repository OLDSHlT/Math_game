using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHitEvent;//�ܻ��¼��������ڵ��ã��ض��ģ��ܻ�����
    public UnityEvent<int, int> healthchanged;
    public UnityEvent deathEvent;
    public GameObject Spoil;//ս��Ʒ
    //����
    Animator animator;
    Rigidbody2D rd;
    //����
    [SerializeField]
    public int _maxHealth = 100;
    public int maxHealth//�������ֵ
    {
        get
        { return _maxHealth; }
        private set
        { _maxHealth = value; }
    }
    [SerializeField]
    public int _Health = 100;
    public int Health//��ǰ����ֵ
    {
        get { return _Health; }
        private set
        {
            value = Mathf.Max(0, value);
            _Health = value;
            healthchanged?.Invoke(_Health, maxHealth);
            if (_Health <= 0)
                isAlive = false;
            
        }
    }
    private bool _isAlive = true;
    public bool isAlive//�Ƿ���
    {
        get { return _isAlive; }
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.isAlive, value);
            if (!_isAlive)
                deathEvent?.Invoke(); 
        }
    }

    public bool LockVelocity//�����ж���Ӳֱ��
    {
        get { return animator.GetBool(AnimationString.LockVelocity); }
        private set { animator.SetBool(AnimationString.LockVelocity, value); }
    }
    //private bool isDefend { get { return animator.GetBool(AnimationString.isDefend); } }//�������������ܷ�
    private bool canKnock //�ܷ� �����ˣ�����/��  ���壩
    { get { return animator.GetBool(AnimationString.canKnock); } }

    private bool isInvincible = false;//�Ƿ����޵�״̬
    public bool RollTriggerInvincible = false;
    public float InvincibieTime = 0.5f;//���˺���޵�ʱ��
    [SerializeField]
    private float Timer = 0;
    // Start is called before the first frame update
    private void Awake()//�Զ���ȡ���
    {
        animator = GetComponent<Animator>();
        rd = GetComponent<Rigidbody2D>();

        //print("start");
    }
    private void Update()
    {   //��Ҫ���ڼ���Ƿ����޵�״̬
        if (RollTriggerInvincible == true)
            isInvincible = true;
        else if (!RollTriggerInvincible && Timer == 0)
        {
            isInvincible = false;
        }
        if (isInvincible && !RollTriggerInvincible)
        {
            Timer += Time.deltaTime;
            if (InvincibieTime <= Timer)
            {
                isInvincible = false;
                Timer = 0;
            }
        }
        Console.WriteLine();
    }
    public bool Hit(int damage, Vector2 knockback)//�ܻ��ж�
    {
        //if (isAlive && !isInvincible && isDefend)         //���ڿ��������÷���״̬
        //    {
        //    Health -= 1;
        //    damageableHitEvent?.Invoke(1, knockback);//?
                            //CharactorEvents.characterDamaged.Invoke(gameObject, 1);
        //    isInvincible = true;
        //    Timer += 0.000001f;
        //    return true;
        //}
        if (isAlive && !isInvincible)
        {
            Health -= damage;
            damageableHitEvent?.Invoke(damage, knockback);
            animator.SetTrigger(AnimationString.HitTrigger);//�����������ö������ġ��ܻ���Trigger

                            //CharactorEvents.characterDamaged.Invoke(gameObject, damage);
            isInvincible = true;
            Timer += 0.000001f;
            return true;
        }
        else
            return false;
    }
    public void OnHit(int damage, Vector2 knockback)//�ܻ������ܣ���ɵ�Ӳֱ�ͻ���
    {
        LockVelocity = true;
        if (canKnock)//���ް���
            rd.velocity = new Vector2(knockback.x, rd.velocity.y + knockback.y);
    }

    public void Heal(int HealthRestore)//����
    {
        if (isAlive && Health < maxHealth)
        {
            int maxheal = Mathf.Max(maxHealth - Health, 0);
            int actulheal = Mathf.Min(maxheal, HealthRestore);
            Health += actulheal;
            //CharactorEvents.characterHealed(gameObject, actulheal);
            
        }
        
    }
    public void FallSpoil()//����ս��Ʒ
    {
        Instantiate(Spoil, transform.position, transform.rotation);
    }
}