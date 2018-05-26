select * from order_item oi join service s on oi.service_id = s.Id
join service_provider sp on s.service_provider_id = sp.Id
where sp.Id = 4 AND oi.is_finished ='N' And oi.is_confirmed = 'Y';

select * from order_item oi join service s on oi.service_id = s.Id
join service_provider sp on s.service_provider_id = sp.Id;

