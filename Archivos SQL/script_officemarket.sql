/*
-- Borrar tablas
drop table producto;
drop table categoria;

-- Borrar secuencias
drop sequence producto_seq;
drop sequence categoria_seq;

-- Borrar triggers
drop trigger al_insertar_producto;
drop trigger al_insertar_categoria;

-- Borrar SP 
drop procedure producto_insertar;
drop procedure producto_actualizar;
drop procedure producto_eliminar;

commit;
*/
-- ==================================================================>>>

create table producto(
    producto_id     number primary key,
    nombre_producto varchar2(100),
    precio_producto number,
    stock_producto  number,
    imagen_producto varchar2(200) null,
    categoria_id    number
);

create table categoria(
    categoria_id number primary key,
    nombre       varchar2(50)
);

alter table producto add foreign key (categoria_id) references categoria (categoria_id);

-- Crear secuencias
CREATE SEQUENCE producto_seq START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE;
CREATE SEQUENCE categoria_seq START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE;

-- ==================================================================>>>
-- Crear triggers
create or replace trigger al_insertar_producto before insert on producto for each row
begin
    select producto_seq.nextval into :new.producto_id from dual;  -- Al insertar un nuevo producto, le aprega su PK
end;

create or replace trigger al_insertar_categoria before insert on categoria for each row
begin
    select categoria_seq.nextval into :new.categoria_id from dual;  -- Al insertar una nueva categoria, le aprega su PK
end;

-- ==================================================================>>>
-- Insertar categorias
insert into categoria (nombre) values ('escritorios');
insert into categoria (nombre) values ('sillas');
insert into categoria (nombre) values ('insumos');
insert into categoria (nombre) values ('art. electronicos');

-- Insertar productos

insert into producto(nombre_producto, precio_producto, stock_producto, imagen_producto, categoria_id)
values('Silla oficina mecanica', 89990, 2000, null, 2);

insert into producto(nombre_producto, precio_producto, stock_producto, imagen_producto, categoria_id)
values('Escritorio terminaci�n cuero', 539990, 10, null, 1);

insert into producto(nombre_producto, precio_producto, stock_producto, imagen_producto, categoria_id)
values('posit', 1990, 3000, null, 3);

insert into producto(nombre_producto, precio_producto, stock_producto, imagen_producto, categoria_id)
values('ventilador de escritorio', 18990, 100, null, 4);

-- ==================================================================>>>

create or replace procedure producto_insertar(
    v_nombre varchar2, 
    v_precio varchar2, 
    v_stock number, 
    v_imagen varchar2, 
    v_categoria_id number,
    id_generado out number   -- Salida del SP
) 
as
begin
    
    insert into producto(nombre_producto, precio_producto, stock_producto, imagen_producto, categoria_id)
    values(v_nombre, v_precio, v_stock, v_imagen, v_categoria_id);
    
    select max(producto_id) into id_generado from producto;
end;


create or replace procedure producto_actualizar(
    v_id number, 
    v_nombre varchar2, 
    v_precio varchar2, 
    v_stock number,
    v_imagen varchar2,
    v_categoria_id number
) 
as
begin

    update producto set 
        nombre_producto = v_nombre,
        precio_producto = v_precio,
        stock_producto  = v_stock,
        imagen_producto = v_imagen,
        categoria_id    = v_categoria_id
    where 
        producto_id = v_id;
    commit;

end;


create or replace procedure producto_eliminar(v_id number) as
begin
    delete from producto where producto_id = v_id;
    commit;
end;


commit;











































